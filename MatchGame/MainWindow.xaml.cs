using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthofSecondsElapsed;
        int matchesFound;


        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetupGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthofSecondsElapsed++;
            timeTextBlock.Text = (tenthofSecondsElapsed / 10F).ToString("0.0s");
        }

        Dictionary<string, int> dict = new Dictionary<string, int>();
        private void SetupGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "😂", "🤣", "😂", "😒", "😂",
                "😂", "🤣", "😂", "😒", "😂",
                "😂", "🤣", "😂", "😒", "😂" 
            };

            for (int x = 0; x < animalEmoji.Count; x++)
            {
                int currentCount;

                dict.TryGetValue(animalEmoji[x], out currentCount);

                dict[animalEmoji[x]] = currentCount + 1;
            }

            int c = 0;
            foreach (TextBlock txt in mainGrid.Children.OfType<TextBlock>())
            {
                if(txt.Name != "timeTextBlock")
                {
                    txt.Text = animalEmoji[c];
                    c++;
                }

            }

            timer.Start();
            tenthofSecondsElapsed = 0;
            matchesFound = 0;


        }

        TextBlock lastTextBlockClikcked;
        bool findingMatch = false;

        string currentSelected = null;
        int countLeft = 0;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (currentSelected == null)
            {
                textBlock.Visibility = Visibility.Hidden;
                currentSelected = textBlock.Text;
                countLeft = dict[currentSelected] - 1;
                if(countLeft == 0)
                {
                    timer.Stop();
                    timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
                }
                lastTextBlockClikcked = textBlock;
                findingMatch = true;

            }
            else if (currentSelected == textBlock.Text && findingMatch == true)
            {
                textBlock.Visibility = Visibility.Hidden;
                matchesFound++;
                countLeft -= 1;
                if(countLeft == 0)
                {
                    timer.Stop();
                    timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
                }
            }
            else if (currentSelected != textBlock.Text && findingMatch == true)
            {
                lastTextBlockClikcked.Visibility = Visibility.Visible;
                countLeft += 1;
            }
        }


        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();

        }
    }
}
