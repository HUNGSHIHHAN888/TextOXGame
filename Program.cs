using static 文字版OXGame.Program;

namespace 文字版OXGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextOXGame game = new TextOXGame();
            game.StartGame();//遊戲開始
        }
        public class OXGameEngine//定義一個類別叫做OXGameEngine
        {
            private char[,] gameMarkers;//存儲遊戲版面的二維陣列狀態

            public OXGameEngine()
            {
                gameMarkers = new char[3, 3];//建立一個大小為3x3的二維陣列來表示遊戲版面
                ResetGame();//重置遊戲
            }

            public void SetMarker(int x, int y, char player)//設置玩家輸入的座標於指定位置
            {
                if (IsValidMove(x, y))//檢查指定位置是否為有效移動
                {
                    gameMarkers[x, y] = player;//標記玩家輸入的座標於指定位置
                }
                else
                {
                    throw new ArgumentException("Invalid move!");//如果移動無效，輸出「無效移動!」提醒玩家
                }
            }

            public void ResetGame()//重置遊戲
            {
                gameMarkers = new char[3, 3];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        gameMarkers[i, j] = ' ';//將每個位置都設置為空白
                    }
                }
            }

            public char IsWinner()//檢查是否有玩家獲勝
            {
                //檢查橫向
                for (int i = 0; i < 3; i++)
                {
                    if (gameMarkers[i, 0] != ' ' && gameMarkers[i, 0] == gameMarkers[i, 1] && gameMarkers[i, 1] == gameMarkers[i, 2])
                    {
                        return gameMarkers[i, 0];//如果有玩家獲勝，回傳該玩家標記O或X
                    }
                }

                //檢查縱向
                for (int j = 0; j < 3; j++)
                {
                    if (gameMarkers[0, j] != ' ' && gameMarkers[0, j] == gameMarkers[1, j] && gameMarkers[1, j] == gameMarkers[2, j])
                    {
                        return gameMarkers[0, j];//如果有玩家獲勝，回傳該玩家標記O或X
                    }
                }

                //檢查對角線
                if (gameMarkers[0, 0] != ' ' && gameMarkers[0, 0] == gameMarkers[1, 1] && gameMarkers[1, 1] == gameMarkers[2, 2])
                {
                    return gameMarkers[0, 0];//如果有玩家獲勝，回傳該玩家標記O或X
                }

                if (gameMarkers[0, 2] != ' ' && gameMarkers[0, 2] == gameMarkers[1, 1] && gameMarkers[1, 1] == gameMarkers[2, 0])
                {
                    return gameMarkers[0, 2];//如果有玩家獲勝，回傳該玩家標記O或X
                }

                return ' ';//沒有贏家出現
            }

            private bool IsValidMove(int x, int y)//檢查指定位置是否為有效移動
            {
                if (x < 0 || x >= 3 || y < 0 || y >= 3)//如果位置超出範圍，則無效
                {
                    return false;
                }

                if (gameMarkers[x, y] != ' ')//如果位置不是空白，則無效
                {
                    return false;
                }

                return true;//有效移動
            }

            public char GetMarker(int x, int y)//獲取指定位置的標記
            {
                return gameMarkers[x, y];//回傳指定位置的標記
            }
        }
    }
    public class TextOXGame//定義一個類別叫做TextOXGame
    {
        private OXGameEngine engine;//設置私有的OXGameEngine，名稱叫engine
        private char currentPlayer;//設置了一個私有變數叫做currentPlayer，並且是char類型

        public TextOXGame()
        {
            engine = new OXGameEngine();
            currentPlayer = 'X'; //由玩家X先開始遊戲
        }

        public void StartGame()//遊戲開始
        {
            do
            {
                Console.Clear();//開始時，版面清空，每個位置都要是空白的
                DisplayBoard();//顯示OXGame文字版面
                Console.WriteLine($"Player {currentPlayer}'s turn.");//顯示目前哪位玩家遊玩
                Console.Write("Enter row number and column number (0-2, separated by space): ");//提醒玩家輸入位置座標(0-2)，並且用空格隔開
                string[] data = Console.ReadLine().Split(" ");//讀取玩家輸入的資料，並分割字串
                int x = int.Parse(data[0]);//轉換字串陣列第一個數字成int
                int y = int.Parse(data[1]);//轉換字串陣列第二個數字成int
                try
                {
                    engine.SetMarker(x, y, currentPlayer);//呼叫遊戲引擎中SetMarker的方法，讓目前的玩家放入輸入的位置的標記
                    Console.Clear();//版面清空
                    DisplayBoard();//顯示OXGame文字版面
                    char winner = engine.IsWinner();//檢查是否有玩家獲勝
                    if (winner != ' ')//如果有玩家獲勝
                    {
                        Console.WriteLine($"Winner: {winner}");//輸出獲勝玩家
                        return;//結束遊戲
                    }
                    currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';//如果沒有玩家獲勝，換另一位玩家
                }
                catch (ArgumentException ex)//檢測到異常，玩家可能輸入無效的座標位置
                {
                    Console.WriteLine(ex.Message);//顯示錯誤訊息
                    Console.WriteLine("Press any key to continue...");//提醒玩家按下鍵盤任意一個鍵繼續遊戲
                    Console.ReadKey();//等待玩家按下鍵盤任意一個鍵
                }

            } while (!IsGameOver());//當遊戲未結束時

            Console.WriteLine("It's a draw!");//顯示平局
        }

        private bool IsGameOver()//檢查遊戲是否結束
        {
            return engine.IsWinner() != ' ' || IsBoardFull();//如果有玩家獲勝或是版面滿了，則遊戲結束
        }

        private bool IsBoardFull()//檢查版面是否都放滿O或X了
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (engine.GetMarker(i, j) == ' ')
                    {
                        return false;//如果找到一個空位，代表版面還沒滿
                    }
                }
            }
            return true;//代表所有位置都滿了
        }

        private void DisplayBoard()//顯示OXGame文字版面
        {
            Console.WriteLine("Game Board:");//設置OXGame版面標題
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(engine.GetMarker(i, j));//標記每個位置是O或X
                    if (j < 2)
                    {
                        Console.Write(" | ");//用「|」分隔每個格子
                    }
                }
                Console.WriteLine();
                if (i < 2)
                {
                    Console.WriteLine("---------");//用「---------」分隔每一行
                }
            }
            Console.WriteLine();
        }
    }
}