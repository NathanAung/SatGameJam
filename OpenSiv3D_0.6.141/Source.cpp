# include <Siv3D.hpp> // Siv3D v0.6.14


enum class GameState
{
	Playing,
	GameOver
};

struct Player {
	const Texture playerT{ U"👮"_emoji };
	int hp = 5;
	Circle collider{ 400, 300, 40 };
	Vec2 currentPos{ 400, 300 };

	void PlayerUpdate() {
		collider.setPos(currentPos);
		//collider.draw();
		if (hp > 0)
			playerT.scaled(0.5).drawAt(collider.center);
	}
};


struct Enemy {
	Circle collider{ 0, 0, 40 };
	int enemyType = 0; // 0: ghost, 1: zombie, 2: vampire
	int hp = 1;
	Vec2 currentPos{ 0,0 };
	Vec2 targetPos = currentPos;
	Vec2 velocity{ 2,2 };
	double speed = 1.5f;
	int score = 10;

	void SetUpEnemy() {
		enemyType = Random(0, 2);

		switch (enemyType) {
		case 1:	// zombie
			hp = 3;
			speed = 1.0f;
			score = 30;
			break;
		case 2:	// vampire
			speed = 2.0f;
			score = 20;
			break;
		}


		int side = Random(0, 3); // 0: left, 1: right, 2: top, 3: bottom

		if (side == 0) { // Left
			currentPos.x = Random(-400, -1);
			currentPos.y = Random(-400, 1000);
		}
		else if (side == 1) { // Right
			currentPos.x = Random(801, 1200);
			currentPos.y = Random(-400, 1000);
		}
		else if (side == 2) { // Top
			currentPos.x = Random(-400, 1200);
			currentPos.y = Random(-400, -1);
		}
		else { // Bottom
			currentPos.x = Random(-400, 1200);
			currentPos.y = Random(601, 1200);
		}
		collider.setPos(currentPos);
	}

	void EnemyUpdate(Vec2 pos, const Texture& texture) {
		targetPos = pos;
		currentPos = MoveTowards(currentPos, targetPos, speed);
		collider.setPos(currentPos);
		//collider.draw();
		texture.scaled(0.5).drawAt(currentPos);
	}

	Vec2 MoveTowards(const Vec2& current, const Vec2& target, double speedDelta)
	{
		Vec2 delta = target - current;
		double distance = delta.length();

		if (distance <= speedDelta || distance == 0.0)
		{
			return target; // Already close enough or at the target
		}

		return current + delta / distance * speedDelta;
	}
};


struct Crosshair {
	const Texture crosshairT{ 0xF04FE_icon, 80 };
	Circle collider{ Cursor::Pos(), 20 };

	void Draw() {
		collider.setPos(Cursor::Pos());
		//collider.draw();
		crosshairT.drawAt(collider.center, ColorF{ 1 });
	}
};


void UpdateEnemies(const Array<Texture>& enemyTexArr, Array<Enemy>& enemies, Player& player, const Crosshair& crosshair, int& score) {
	if (player.hp <= 0)
		return; // Skip updates if the player is dead

	for (int i = enemies.size() - 1; i >= 0; --i) {
		Enemy& enemy = enemies[i];
		enemy.EnemyUpdate(player.collider.center, enemyTexArr[enemy.enemyType]);

		if (enemy.collider.intersects(player.collider)) {
			player.hp = static_cast<int>(Math::Max(0, static_cast<double>(player.hp - 1)));
			enemies.remove_at(i);
			continue; // Skip further checks for this enemy
		}
		else if (enemy.collider.intersects(crosshair.collider) && MouseL.down()) {
			enemy.hp -= 1;
			if (enemy.hp <= 0) {
				score += enemy.score;
				enemies.remove_at(i);
				// No need for continue here, as it's the last check
			}
		}
	}

	//for(auto e = enemies.begin(); e != enemies.end();) {
	//	if(player.collider.intersects(e->collider)) {
	//		player.hp = static_cast<int>(Math::Max(0, static_cast<double>(player.hp - 1)));
	//		e = enemies.erase(e); // Remove enemy and get the next iterator
	//	}
	//	else {
	//		++e; // Only increment if not erasing
	//	}
	//}
}


void CreateEnemies(const double& deltaTime, const double& currentTime, double& genTimer, Array<Enemy>& enemies, const GameState& state) {
	if (state == GameState::GameOver)
		return;

	static double genTime = 0.5f;

	genTimer += deltaTime;

	if (genTimer >= genTime) {
		enemies.emplace_back();           // Construct Enemy in place
		enemies.back().SetUpEnemy();      // Set up the newly created Enemy
		genTimer = 0.0;
	}
}


void Test()
{
	Scene::SetBackground(ColorF{ 0.2, 0.2, 0.2 });

	GameState gameState = GameState::Playing;

	const Font font{ FontMethod::MSDF, 48, Typeface::Bold };

	const Array<Texture> enemyTexArr =
	{
		Texture{ U"👻"_emoji },
		Texture{ U"🧟"_emoji },
		Texture{ U"🧛‍♀️"_emoji }
	};

	Player player;
	Array<Enemy> enemies;
	Crosshair crosshair;

	int score = 0;

	double time = 0.0;
	double genTimer = 0.0;

	//for (int i = 0; i < 5; i++)
	//{
	//	enemies.emplace_back();           // Construct Enemy in place
	//	enemies.back().SetUpEnemy();      // Set up the newly created Enemy
	//}


	while (System::Update())
	{
		const double deltaTime = Scene::DeltaTime();
		time += deltaTime;

		// hide cursor
		Cursor::RequestStyle(CursorStyle::Hidden);

		UpdateEnemies(enemyTexArr, enemies, player, crosshair, score);
		CreateEnemies(deltaTime, time, genTimer, enemies, gameState);

		player.PlayerUpdate();

		crosshair.Draw();

		font(U"HP: {}"_fmt(player.hp)).draw(32, Vec2{ 20, 20 }, ColorF{ 1 });
		font(U"Score: {}"_fmt(score)).draw(32, Vec2{ 20, 70 }, ColorF{ 1 });

		if (player.hp <= 0)
		{
			font(U"GAME OVER").drawAt(64, Vec2{ 400, 300 }, ColorF{ 0.7, 0.0, 0.0 });
			if (gameState == GameState::Playing)
				gameState = GameState::GameOver;
		}
	}
}
