# include <Siv3D.hpp>

enum class GameState
{
	Start,
	Playing,
	BadEnd,     // exploded in-room
	GoodEnd     // bomb disabled + escaped
};


// BUTTON FUNCTION
bool Button(const Rect& rect, const Font& font, const String& text)
{
	// If the mouse cursor is over the button
	if (rect.mouseOver())
	{
		// Change the mouse cursor to a hand
		Cursor::RequestStyle(CursorStyle::Hand);
	}

	const RoundRect roundRect = rect.rounded(6);

	// Draw shadow and background
	roundRect.draw(ColorF{ 1.0 });

	// Draw text
	font(text).drawAt(16, rect.center(), ColorF{ 0.0 });

	// Return true if the button is pressed
	return rect.leftClicked();
}


void Main()
{
	Window::SetTitle(U"脱出ゲーム");

	ColorF bgColorDefault{ 0.0, 0.0, 0.0 };
	ColorF bgColorGood{ 0.278, 0.78, 0.0 };
	ColorF bgColorBad{ 0.7, 0.0, 0.0 };
	Scene::SetBackground(bgColorDefault);

	GameState gameState = GameState::Start;

	const Font font{ FontMethod::MSDF, 48, Typeface::Bold };
	// starting dialogues
	Array<String> startDialogues = { U".....",
		U"暗い部屋で目を覚ました。\n頭がズキズキと痛む。",
		U"目が慣れてくると、机の上から\nかすかな赤い光が見える。",
		U"そこには小さな金属の箱が置かれている。\nタイマー付きだ。",
		U"タイマーは 05:00 を示している。",
		U"...爆弾だ。" };
	String leftWallText = U"天井から3つの電球がぶら下がっている。1つは壊れて光っていない。";
	String centerWallText = U"カレンダーは3月19日に印がついている。";
	String rightWallText = U"時計は3時5分で止まっており、分針がわずかに震えている。";
	String hintText = U"爆弾の下に小さなメモがある。そこにはこう書かれている、\n「生きているものだけが価値を持ち、三つ目の影には何もない」。";

	String* text = &startDialogues[0];
	// for printing text
	Stopwatch stopwatch;

	bool endTextPrinted = false;

	bool printText = true;
	bool finishedPrinting = false;
	int32 currentDialogueIndex = 0;

	// 0 = main, 1 = bomb
	int32 currentMenuIndex = 0;


	//BOMB
	bool showBombTimer = false;
	bool bombStarted = false;
	float bombTimer = 300.0f; // 5 minutes in seconds
	String bombCode = U"2195";
	String enteredCodeString; // String to hold the entered code
	bool typingCode = false; // Whether the player is currently typing the code
	bool redWireCut = false;
	
	Rect inputArea{ 350, 190, 100, 40 }; // Area for inputting the bomb code




	while (System::Update())
	{
		// Elapsed time from previous frame (seconds)
		const double deltaTime = Scene::DeltaTime();

		if (KeyEscape.down()) {
			break; // Exit the game
		}

		// TEXT PRINTING
		if (printText) {
			if (!finishedPrinting && !stopwatch.isRunning()) {
				stopwatch.restart();
			}
			else {
				const int32 count = (stopwatch.ms() / 60);
				if (finishedPrinting) {
					font(*text).drawAt(16, Vec2{ 400, 300 }, ColorF{ 1.0 });
				}
				else {
					font(text->substr(0, count)).drawAt(16, Vec2{ 400, 300 }, ColorF{ 1.0 });
				}

				if (count > text->length()) {
					finishedPrinting = true;
					stopwatch.pause();
				}
			}
		}

		// BOMB TIMER
		if (showBombTimer) {
			if (bombStarted) {
				bombTimer -= deltaTime;
				if (bombTimer <= 0.0f) {
					gameState = GameState::BadEnd;
					Scene::SetBackground(bgColorBad);
					bombTimer = 0.0f; // Prevent negative display
				}
			}
			int minutes = static_cast<int>(bombTimer) / 60;
			int seconds = static_cast<int>(bombTimer) % 60;
			font(U"{:02}:{:02}"_fmt(minutes, seconds)).drawAt(32, Vec2{ 400, 50 }, ColorF{ 1.0, 0.0, 0.0 });
		}


		// GAME STATE LOGIC
		// GAME START
		if (gameState == GameState::Start) {

			if (MouseL.down()) {
				// Skip to the end of the text if still printing
				if (!stopwatch.isPaused()) {
					finishedPrinting = true;
					stopwatch.pause();
				}
				// Move to the next dialogue
				else if (currentDialogueIndex < startDialogues.size() - 1) {
					stopwatch.restart();
					currentDialogueIndex++;
					text = &startDialogues[currentDialogueIndex];
					finishedPrinting = false;

					if (currentDialogueIndex == 4) {
						showBombTimer = true; // Show the bomb timer when the player sees the box
					}
				}
				else {
					printText = false;
					bombStarted = true;
					gameState = GameState::Playing; // Move to the main game state
				}
			}
		}
		// MAIN GAME
		else if (gameState == GameState::Playing) {
			switch (currentMenuIndex)
			{
			// main menu
			case 0:
				if (Button(Rect{ 180, 400, 200, 50 }, font, U"爆弾を見る"))
				{
					stopwatch.restart();
					*text = hintText;
					finishedPrinting = false;
					printText = true;
					currentMenuIndex = 1; // Switch to bomb menu
				}
				if (Button(Rect{ 420, 400, 200, 50 }, font, U"左を見る"))
				{
					stopwatch.restart();
					*text = leftWallText;
					finishedPrinting = false;
					printText = true;
				}
				if (Button(Rect{ 180, 480, 200, 50 }, font, U"右を見る"))
				{
					stopwatch.restart();
					*text = rightWallText;
					finishedPrinting = false;
					printText = true;
				}
				if (Button(Rect{ 420, 480, 200, 50 }, font, U"真ん中を見る"))
				{
					stopwatch.restart();
					*text = centerWallText;
					finishedPrinting = false;
					printText = true;
				}
				break;
			// bomb menu
			case 1:
				if (typingCode) {
					if (KeyEnter.down()) {
						if(enteredCodeString == bombCode) {
							gameState = GameState::GoodEnd; // Correct code, go to good end
							Scene::SetBackground(bgColorGood);
						}
						else {
							gameState = GameState::BadEnd; // Incorrect code, go to bad end
							Scene::SetBackground(bgColorBad);
						}
					}
					else {
						// Input text from keyboard
						TextInput::UpdateText(enteredCodeString);

						// Get unconverted character input
						const String editingText = TextInput::GetEditingText();

						inputArea.draw(ColorF{ 0.3 });

						font(enteredCodeString + U'|' + editingText).draw(16, inputArea.stretched(-1));
					}
				}
				if (Button(Rect{ 180, 400, 200, 50 }, font, U"解除コードを打つ"))
				{
					typingCode = true; // Start typing code
				}
				if (Button(Rect{ 420, 400, 200, 50 }, font, U"赤いコードを切る"))
				{
					typingCode = false; // Stop typing code
					if(!redWireCut) {
						redWireCut = true; // Cut the red wire
						stopwatch.restart();
						*text = U".....何も起こらなかった。";
						finishedPrinting = false;
						printText = true;
					} else {
						stopwatch.restart();
						*text = U"赤いコードはすでに切られている。";
						finishedPrinting = false;
						printText = true;
					}
				}
				if (Button(Rect{ 180, 480, 200, 50 }, font, U"青いコードを切る"))
				{
					typingCode = false; // Stop typing code
					gameState = GameState::BadEnd; // bomb exploded
					Scene::SetBackground(bgColorBad);
					
				}
				if (Button(Rect{ 420, 480, 200, 50 }, font, U"戻る"))
				{
					printText = false; // Stop printing text
					typingCode = false; // Stop typing code
					currentMenuIndex = 0; // Go back to the main menu
				}
				break;
			default:
				break;
			}

		}
		// BAD END
		else if (gameState == GameState::BadEnd) {
			if (!endTextPrinted) {
				showBombTimer = false; // Stop showing the bomb timer
				stopwatch.restart();
				*text = U"爆弾が爆発し、あなたは死んだ。";
				finishedPrinting = false;
				printText = true;
				endTextPrinted = true;
			}
		}
		// GOOD END
		else if (gameState == GameState::GoodEnd) {
			if (!endTextPrinted) {
				showBombTimer = false; // Stop showing the bomb timer
				stopwatch.restart();
				*text = U"爆弾を解除して部屋から脱出した！";
				finishedPrinting = false;
				printText = true;
				endTextPrinted = true;
			}
		}


	}
}
