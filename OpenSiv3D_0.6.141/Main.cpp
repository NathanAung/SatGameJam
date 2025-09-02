# include <Siv3D.hpp> // OpenSiv3D v0.6.3

void Main()
{
	// game title
	Window::SetTitle(U"Basketball Game");

	// Background color (sky blue)
	Scene::SetBackground(ColorF{ 0.4, 0.7, 1.0 });

	// --- Physics setup ---
	constexpr double StepSec = (1.0 / 200.0);  // Fixed timestep (stable simulation)
	double accumulatorSec = 0.0;
	P2World world;

	// --- Ground setup ---
	const P2Body ground = world.createRect(
		P2Static, Vec2{ 100, 600 }, SizeF{ 1600, 200 },
		P2Material{ .density = 20.0, .restitution = 0.02, .friction = 0.8 }
	);

	// --- Hoop setup ---
	const Vec2 hoopPos{ 700, 200 };
	const SizeF rimSize{ 100, 10 };
	const SizeF backboardSize{ 10, 100 };

	// Backboard is positioned relative to hoop
	const P2Body backboard = world.createRect(
		P2Static,
		hoopPos + Vec2{ rimSize.x / 2 - backboardSize.x, -backboardSize.y / 2 + rimSize.y / 2 },
		backboardSize,
		P2Material{}
	);

	// --- Scoring zone (detect if ball passes through rim) ---
	RectF scoreZone{ hoopPos.x - 65, hoopPos.y - 15, rimSize.x, 20 };
	int32 score = 0;

	// --- Ball setup ---
	Array<P2Body> balls;
	constexpr double BallRadius = 20;
	constexpr Circle StartCircle{ 200, 300, BallRadius }; // where you grab/launch the ball
	Optional<Vec2> grabbed; // grab position
	constexpr Duration CoolTime = 0.3s; // cooldown between shots
	Stopwatch timeSinceShot{ CoolTime, StartImmediately::Yes };

	// --- UI setup ---
	const Font font{ FontMethod::MSDF, 48, Typeface::Bold };
	const Texture ballT{ U"🏀"_emoji }; // ball texture (emoji)
	const double BallTexSize = BallRadius * 2; // texture scale to match physics radius

	while (System::Update())
	{
		const bool readyToLaunch = (CoolTime <= timeSinceShot);

		// --- Physics update with fixed timestep ---
		for (accumulatorSec += Scene::DeltaTime(); StepSec <= accumulatorSec; accumulatorSec -= StepSec)
		{
			world.update(StepSec);
		}

		// Remove balls that fall below screen
		balls.remove_if([](const P2Body& b) { return (700 < b.getPos().y); });

		// --- Ball grab / launch logic ---
		if (readyToLaunch && StartCircle.leftClicked())
		{
			grabbed = Cursor::PosF();
		}

		Vec2 ballPos = StartCircle.center; // default spawn position
		Vec2 ballDelta{ 0,0 };             // launch vector

		// While dragging the mouse, calculate launch vector
		if (grabbed)
		{
			ballDelta = (*grabbed - Cursor::PosF()).limitLength(110);
			ballPos -= ballDelta;
		}

		// Release mouse → launch ball
		if (grabbed && MouseL.up())
		{
			P2Body newBall = world.createCircle(
				P2Dynamic, ballPos, BallRadius,
				P2Material{ .density = 100.0, .restitution = 0.0, .friction = 1.0 }
			).setVelocity(ballDelta * 8);

			balls << newBall;
			grabbed.reset();
			timeSinceShot.restart();
		}

		// --- Scoring check ---
		for (auto& ball : balls)
		{
			if (scoreZone.intersects(ball.getPos()))
			{
				score++;
				ball.setAwake(false);               // stop physics
				ball.setPos(Vec2{ 9999, 9999 });    // move out of view
			}
		}

		// --- Drawing section ---

		// Ground
		const Quad groundQuad = ground.as<P2Rect>(0)->getQuad();
		const RectF groundRect{ groundQuad.p0, (groundQuad.p2 - groundQuad.p0) };
		groundRect.draw(ColorF{ 0.4, 0.2, 0.0 }) // brown dirt
			.drawFrame(40, 0, ColorF{ 0.2, 0.8, 0.4 }); // green grass

		// Hoop backboard + score zone
		backboard.as<P2Rect>(0)->getQuad().draw(ColorF{ 0.8 });
		scoreZone.drawFrame(2, 0, ColorF{ 0.0, 1.0, 0.0, 0.5 });

		// Balls already launched
		for (const auto& ball : balls)
		{
			ballT.resized(BallTexSize).drawAt(ball.getPos());
		}

		// Cursor style feedback
		if (readyToLaunch && (grabbed || StartCircle.mouseOver()))
		{
			Cursor::RequestStyle(CursorStyle::Hand);
		}

		// Start circle (spawn point)
		StartCircle.drawFrame(2);

		// Preview ball while dragging
		if (readyToLaunch)
		{
			Circle{ ballPos, BallRadius }.draw();
			ballT.resized(BallTexSize).drawAt(ballPos);
		}

		// Draw arrow for launch strength
		if (20.0 < ballDelta.length())
		{
			Line{ ballPos, (ballPos + ballDelta) }
				.stretched(-10)
				.drawArrow(10, { 20, 20 }, ColorF{ 1.0, 0.0, 0.0, 0.5 });
		}

		// Predicted trajectory preview
		if (not ballDelta.isZero())
		{
			const Vec2 v0 = (ballDelta * 8);
			for (int32 i = 1; i <= 10; ++i)
			{
				const double t = (i * 0.15);
				const Vec2 pos = ballPos + (v0 * t) + (0.5 * world.getGravity() * t * t);
				Circle{ pos, 6 }.draw(ColorF{ 1.0, 0.6 }).drawFrame(3);
			}
		}

		// Score text
		font(U"Score: {}"_fmt(score)).draw(32, 10, 10, ColorF{ 1.0 });
	}
}
