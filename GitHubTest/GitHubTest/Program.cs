namespace GitHubTest
{
    internal class Program
    {
        // 소코반 게임
        // 매커니즘 추가: 스테이지 클리어, 움직인 횟수 카운트
        struct Position
        {
            public int x;
            public int y;
        }

        static void Main(string[] args)
        {
            bool stage1Clear = false;
            // 스테이지 1 클리어 여부
            bool stage2Clear = false;

            Position playerPos;

            playerPos.x = 0;
            playerPos.y = 0;

            // 몇 번 움직였는 지 카운트
            int moveCount = 0;

            char[,] map;

            Start(ref playerPos, out map, stage1Clear);

            while (stage1Clear == false)
            {
                Render(ref playerPos, map, moveCount);
                ConsoleKey key = Input();
                Update(ref playerPos, key, map, ref stage1Clear, ref stage2Clear, ref moveCount);
            }

            NextStageSelection(moveCount, stage1Clear, ref stage2Clear);

            Start(ref playerPos, out map, stage1Clear);
            // 스테이지 1 움직인 횟수 기록
            int stage1moveCount = moveCount;
            // 움직인 횟수 초기화
            moveCount = 0;

            while (stage2Clear == false)
            {
                Render(ref playerPos, map, moveCount);
                ConsoleKey key = Input();
                Update(ref playerPos, key, map, ref stage1Clear, ref stage2Clear, ref moveCount);
            }
            End(moveCount, stage1moveCount, stage1Clear, stage2Clear);
        }

        static void Start(ref Position playerPos, out char[,] map, bool stage1Clear)
        {
            Console.CursorVisible = false;

            // 플레이어 위치 설정
            playerPos.x = 4;
            playerPos.y = 4;

            // 맵 설정
            map = new char[,]
            {
                    { ' ', ' ', '#', '#', '#', ' ', ' ', ' ' },
                    { ' ', ' ', '#', '.', '#', ' ', ' ', ' ' },
                    { ' ', ' ', '#', ' ', '#', '#', '#', '#' },
                    { '#', '#', '#', '@', ' ', '@', '.', '#' },
                    { '#', '.', ' ', '@', ' ', '#', '#', '#' },
                    { '#', '#', '#', '#', '@', '#', ' ', ' ' },
                    { ' ', ' ', ' ', '#', '.', '#', ' ', ' ' },
                    { ' ', ' ', ' ', '#', '#', '#', ' ', ' ' },
            };

            if (stage1Clear)
            {
                Console.Clear();
                // 스테이지 2 위치 설정
                playerPos.x = 11;
                playerPos.y = 8;

                // 스테이지 2 맵 설정
                map = new char[,]
                {
                { ' ', ' ', ' ', ' ', '#', '#', '#', '#', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', ' ', ' ', '#', '@', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', '#', '#', '#', ' ', ' ', '@', '#', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                { ' ', ' ', '#', ' ', ' ', '@', ' ', '@', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                { '#', '#', '#', ' ', '#', ' ', '#', '#', ' ', '#', ' ', ' ', ' ', '#', '#', '#', '#', '#', '#' },
                { '#', ' ', ' ', ' ', '#', ' ', '#', '#', ' ', '#', '#', '#', '#', '#', ' ', ' ', '.', '.', '#' },
                { '#', ' ', '@', ' ', ' ', '@', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '.', '.', '#' },
                { '#', '#', '#', '#', '#', ' ', '#', '#', '#', ' ', '#', ' ', '#', '#', ' ', ' ', '.', '.', '#' },
                { ' ', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', '#', '#', '#', '#', '#', '#', '#', '#', '#' },
                { ' ', ' ', ' ', ' ', '#', '#', '#', '#', '#', '#', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                };
            }



        }

        static void Render(ref Position playerPos, char[,] map, int moveCount)
        {
            Console.SetCursorPosition(0, 0);
            // 맵 표시
            PrintMap(map);
            // 플레이어 표시
            PrintPlayer(playerPos, map);
            // 움직임 카운트 표시
            PrintMoveCount(moveCount);

            static void PrintMap(char[,] map)
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        Console.Write(map[y, x]);
                    }
                    Console.WriteLine("");
                }
            }


            static void PrintPlayer(Position playerPos, char[,] map)
            {
                Console.SetCursorPosition(playerPos.x, playerPos.y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write('P');
                Console.ResetColor();
            }

            static void PrintMoveCount(int MoveCount)
            {
                Console.SetCursorPosition(0, 15);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"움직인 횟수: {MoveCount}");
                Console.ResetColor();
                Console.SetCursorPosition(0, 0);
            }
        }


        static ConsoleKey Input()
        {
            // 키 입력
            ConsoleKey input = Console.ReadKey(true).Key;
            return input;
        }

        static void Update(ref Position playerPos, ConsoleKey key, char[,] map, ref bool stage1Clear, ref bool stage2Clear, ref int moveCount)
        {

            // 이동
            Move(ref playerPos, key, map, ref moveCount);

            // 게임 클리어
            bool success1 = Success1(map);
            bool success2 = Success2(map);

            if (success1)
            {
                stage1Clear = true;
            }
            if (success2)
            {
                stage2Clear = true;
            }


            // 이동
            static void Move(ref Position playerPos, ConsoleKey key, char[,] map, ref int moveCount)
            {

                switch (key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        {
                            // 1. 진행방향에 공백 - 움직임
                            if (map[playerPos.y - 1, playerPos.x] == ' ')
                            {
                                playerPos.y--;
                                moveCount++;
                            }
                            // 2. 진행방향에 골 - 움직임
                            else if (map[playerPos.y - 1, playerPos.x] == '.')
                            {
                                playerPos.y--;
                                moveCount++;
                            }
                            // 3. 진행방향에 벽 - 부동
                            else if (map[playerPos.y - 1, playerPos.x] == '#')
                            {

                            }
                            // 4. 진행방향에 박스

                            else if (map[playerPos.y - 1, playerPos.x] == '@')
                            {
                                // 4.1 진행방향 2칸에 공백
                                if (map[playerPos.y - 2, playerPos.x] == ' ')
                                {
                                    map[playerPos.y - 1, playerPos.x] = ' ';
                                    map[playerPos.y - 2, playerPos.x] = '@';
                                    playerPos.y--;
                                    moveCount++;
                                }
                                // 4.2 진행방향 2칸에 골
                                else if (map[playerPos.y - 2, playerPos.x] == '.')
                                {
                                    map[playerPos.y - 1, playerPos.x] = ' ';
                                    map[playerPos.y - 2, playerPos.x] = '*';
                                    playerPos.y--;
                                    moveCount++;
                                }
                                // 4.3 진행방향 2칸에 벽, 박스, 골+박스 - 안움직임
                                else
                                {

                                }
                            }
                            // 5. 진행방향에 골+박스
                            else if (map[playerPos.y - 1, playerPos.x] == '*')
                            {
                                // 5-1. 진행방향 2칸에 공백
                                if (map[playerPos.y - 2, playerPos.x] == ' ')
                                {
                                    map[playerPos.y - 2, playerPos.x] = '@';
                                    map[playerPos.y - 1, playerPos.x] = '.';
                                    playerPos.y--;
                                    moveCount++;
                                }
                                // 5-2. 진행방향 2칸에 골
                                else if (map[playerPos.y - 2, playerPos.x] == '.')
                                {
                                    map[playerPos.y - 2, playerPos.x] = '*';
                                    map[playerPos.y - 1, playerPos.x] = '.';
                                    playerPos.y--;
                                    moveCount++;
                                }
                                // 5-3. 진행방향 2칸에 벽, 박스, 골+박스 - 안움직임
                                else
                                {

                                }
                            }
                            break;
                        }
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        {
                            // 1. 진행방향에 공백 - 움직임
                            if (map[playerPos.y + 1, playerPos.x] == ' ')
                            {
                                playerPos.y++;
                                moveCount++;
                            }
                            // 2. 진행방향에 골 - 움직임
                            else if (map[playerPos.y + 1, playerPos.x] == '.')
                            {
                                playerPos.y++;
                                moveCount++;
                            }
                            // 3. 진행방향에 벽 - 부동
                            else if (map[playerPos.y + 1, playerPos.x] == '#')
                            {

                            }
                            // 4. 진행방향에 박스

                            else if (map[playerPos.y + 1, playerPos.x] == '@')
                            {
                                // 4.1 진행방향 2칸에 공백
                                if (map[playerPos.y + 2, playerPos.x] == ' ')
                                {
                                    map[playerPos.y + 1, playerPos.x] = ' ';
                                    map[playerPos.y + 2, playerPos.x] = '@';
                                    playerPos.y++;
                                    moveCount++;
                                }
                                // 4.2 진행방향 2칸에 골
                                else if (map[playerPos.y + 2, playerPos.x] == '.')
                                {
                                    map[playerPos.y + 1, playerPos.x] = ' ';
                                    map[playerPos.y + 2, playerPos.x] = '*';
                                    playerPos.y++;
                                    moveCount++;
                                }
                                // 4.3 진행방향 2칸에 벽, 박스, 골+박스 - 안움직임
                                else
                                {

                                }
                            }
                            // 5. 진행방향에 골+박스
                            else if (map[playerPos.y + 1, playerPos.x] == '*')
                            {
                                // 5-1. 진행방향 2칸에 공백
                                if (map[playerPos.y + 2, playerPos.x] == ' ')
                                {
                                    map[playerPos.y + 2, playerPos.x] = '@';
                                    map[playerPos.y + 1, playerPos.x] = '.';
                                    playerPos.y++;
                                    moveCount++;
                                }
                                // 5-2. 진행방향 2칸에 골
                                else if (map[playerPos.y + 2, playerPos.x] == '.')
                                {
                                    map[playerPos.y + 2, playerPos.x] = '*';
                                    map[playerPos.y + 1, playerPos.x] = '.';
                                    playerPos.y++;
                                    moveCount++;
                                }
                                // 5-3. 진행방향 2칸에 벽, 박스, 골+박스 - 안움직임
                                else
                                {

                                }
                            }
                            break;
                        }
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        {
                            // 1. 진행방향에 공백 - 움직임
                            if (map[playerPos.y, playerPos.x - 1] == ' ')
                            {
                                playerPos.x--;
                                moveCount++;
                            }
                            // 2. 진행방향에 골 - 움직임
                            else if (map[playerPos.y, playerPos.x - 1] == '.')
                            {
                                playerPos.x--;
                                moveCount++;
                            }
                            // 3. 진행방향에 벽 - 부동
                            else if (map[playerPos.y, playerPos.x - 1] == '#')
                            {

                            }
                            // 4. 진행방향에 박스

                            else if (map[playerPos.y, playerPos.x - 1] == '@')
                            {
                                // 4.1 진행방향 2칸에 공백
                                if (map[playerPos.y, playerPos.x - 2] == ' ')
                                {
                                    map[playerPos.y, playerPos.x - 1] = ' ';
                                    map[playerPos.y, playerPos.x - 2] = '@';
                                    playerPos.x--;
                                    moveCount++;
                                }
                                // 4.2 진행방향 2칸에 골
                                else if (map[playerPos.y, playerPos.x - 2] == '.')
                                {
                                    map[playerPos.y, playerPos.x - 1] = ' ';
                                    map[playerPos.y, playerPos.x - 2] = '*';
                                    playerPos.x--;
                                    moveCount++;
                                }
                                // 4.3 진행방향 2칸에 벽, 박스, 골+박스 - 안움직임
                                else
                                {

                                }
                            }
                            // 5. 진행방향에 골+박스
                            else if (map[playerPos.y, playerPos.x - 1] == '*')
                            {
                                // 5-1. 진행방향 2칸에 공백
                                if (map[playerPos.y, playerPos.x - 2] == ' ')
                                {
                                    map[playerPos.y, playerPos.x - 2] = '@';
                                    map[playerPos.y, playerPos.x - 1] = '.';
                                    playerPos.x--;
                                    moveCount++;
                                }
                                // 5-2. 진행방향 2칸에 골
                                else if (map[playerPos.y, playerPos.x - 2] == '.')
                                {
                                    map[playerPos.y, playerPos.x - 2] = '*';
                                    map[playerPos.y, playerPos.x - 1] = '.';
                                    playerPos.x--;
                                    moveCount++;
                                }
                                // 5-3. 진행방향 2칸에 벽, 박스, 골+박스 - 안움직임
                                else
                                {

                                }
                            }
                            break;
                        }
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        {
                            // 1. 진행방향에 공백 - 움직임
                            if (map[playerPos.y, playerPos.x + 1] == ' ')
                            {
                                playerPos.x++;
                                moveCount++;
                            }
                            // 2. 진행방향에 골 - 움직임
                            else if (map[playerPos.y, playerPos.x + 1] == '.')
                            {
                                playerPos.x++;
                                moveCount++;
                            }
                            // 3. 진행방향에 벽 - 부동
                            else if (map[playerPos.y, playerPos.x + 1] == '#')
                            {

                            }
                            // 4. 진행방향에 박스

                            else if (map[playerPos.y, playerPos.x + 1] == '@')
                            {
                                // 4.1 진행방향 2칸에 공백
                                if (map[playerPos.y, playerPos.x + 2] == ' ')
                                {
                                    map[playerPos.y, playerPos.x + 1] = ' ';
                                    map[playerPos.y, playerPos.x + 2] = '@';
                                    playerPos.x++;
                                    moveCount++;
                                }
                                // 4.2 진행방향 2칸에 골
                                else if (map[playerPos.y, playerPos.x + 2] == '.')
                                {
                                    map[playerPos.y, playerPos.x + 1] = ' ';
                                    map[playerPos.y, playerPos.x + 2] = '*';
                                    playerPos.x++;
                                    moveCount++;
                                }
                                // 4.3 진행방향 2칸에 벽, 박스, 골+박스 - 안움직임
                                else
                                {

                                }
                            }
                            // 5. 진행방향에 골+박스
                            else if (map[playerPos.y, playerPos.x + 1] == '*')
                            {
                                // 5-1. 진행방향 2칸에 공백
                                if (map[playerPos.y, playerPos.x + 2] == ' ')
                                {
                                    map[playerPos.y, playerPos.x + 2] = '@';
                                    map[playerPos.y, playerPos.x + 1] = '.';
                                    playerPos.x++;
                                    moveCount++;
                                }
                                // 5-2. 진행방향 2칸에 골
                                else if (map[playerPos.y, playerPos.x + 2] == '.')
                                {
                                    map[playerPos.y, playerPos.x + 2] = '*';
                                    map[playerPos.y, playerPos.x + 1] = '.';
                                    playerPos.x++;
                                    moveCount++;
                                }
                                // 5-3. 진행방향 2칸에 벽, 박스, 골+박스 - 안움직임
                                else
                                {

                                }
                            }
                            break;
                        }
                }
            }

            static bool Success1(char[,] map)
            {
                //골 6개 클리어
                int goalCount1 = 0;

                foreach (char c in map)
                {
                    if (c == '*')
                    {
                        goalCount1++;
                    }
                }
                if (goalCount1 == 4)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            static bool Success2(char[,] map)
            {
                //골 4개 클리어
                int goalCount2 = 0;

                foreach (char c in map)
                {
                    if (c == '*')
                    {
                        goalCount2++;
                    }
                }
                if (goalCount2 == 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        static void NextStageSelection(int moveCount, bool stage1Clear, ref bool stage2Clear)
        {
            Console.Clear();
            Console.WriteLine("게임 클리어!");
            Console.WriteLine("축하합니다!");
            Console.WriteLine($"움직인 횟수는 {moveCount}입니다.");
            Console.WriteLine("다음 스테이지로 넘어가시겠습니까?");
            Console.WriteLine("1. 예\t 2. 아니요");

            while (true)
            {
                bool isAnswerNum = int.TryParse(Console.ReadLine(), out int numAnswer);
                if (isAnswerNum == false)
                {
                    Console.WriteLine("숫자로 입력해주세요!");
                }
                else if (numAnswer > 2 || numAnswer < 1)
                {
                    Console.WriteLine("숫자 1 혹은 2를 입력해주세요!");
                }

                if (numAnswer == 1)
                {
                    break;
                }
                else if (numAnswer == 2)
                {
                    stage2Clear = true;
                    break;
                }
            }


        }

        static void End(int moveCount, int stage1moveCount, bool stage1Clear, bool stage2Clear)
        {
            if (stage1Clear == true && stage2Clear == true)
            {
                if (moveCount == 0)
                {
                    return;
                }
                else if (moveCount > 0)
                {
                    Console.Clear();
                    Console.WriteLine("게임 클리어!");
                    Console.WriteLine("축하합니다!");
                    Console.WriteLine($"1 스테이지의  움직인 횟수는{stage1moveCount}입니다.");
                    Console.WriteLine($"2 스테이지의  움직인 횟수는{moveCount}입니다.");
                }
            }



        }
    }
}
