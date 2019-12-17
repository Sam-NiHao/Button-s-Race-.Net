using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BTNs__Race
{
    public delegate void HelpToCallMethodAsync(Button button);
    public partial class MainForm : Form
    {
        ManualResetEvent pauseEvent = new ManualResetEvent(true);

        Thread t1;
        Thread t2;
        Thread t3;

        HelpToCallMethodAsync helper;

        static Random rnd;

        CompareButtons[] buttons;

        public MainForm()
        {
            InitializeComponent();
            helper = new HelpToCallMethodAsync(Motion);
            rnd = new Random();
            ButtonsOnOff(false);
            buttons = new CompareButtons[] {buttonOne, buttonTwo, buttonThree};
        }

        private void ButtonsOnOff(bool flag)
        {
            buttonPause.Enabled = flag;
            buttonStop.Enabled = flag;
        }

        public void Pause()
        {
            pauseEvent.Reset();
        }

        public void Resume()
        {
            pauseEvent.Set();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ButtonStart(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;

            ButtonsOnOff(true);

            if (t1 != null)
            {
                //t1.Resume();
                //t2.Resume();
                //t3.Resume();

                Resume();

                return;
            }

            t1 = new Thread(MovementButtonOne);
            t2 = new Thread(MovementButtonTwo);
            t3 = new Thread(MovementButtonThree);

            t1.IsBackground = t2.IsBackground = t3.IsBackground = true;

            t1.Start();
            t2.Start();
            t3.Start();
        }

        private void Motion(Button button)
        {
            button.Location = new Point(button.Location.X + rnd.Next(5), button.Location.Y);

            WhereIsLider();

            Finish(button);
        }

        private void Finish(Button button)
        {
            if(button.Location.X > (pictureBox.Location.X - button.Width))
            {
                ButtonPause(new object(), new EventArgs());

                buttonStart.Enabled = false;
            }
        }

        private void WhereIsLider()
        {
            Array.Sort(buttons);
            buttons[0].BackColor = Color.Yellow;

            for (int i = 1; i < buttons.Length; i++)
            {
                buttons[i].BackColor = SystemColors.Control;
            }
        }

        private void MovementButtonOne()
        {
            MovementLogic(buttonOne);
        }
        private void MovementButtonTwo()
        {
            MovementLogic(buttonTwo);
        }
        private void MovementButtonThree()
        {
            MovementLogic(buttonThree);
        }

        private void MovementLogic(Button button)
        {
            while (true)
            {
                pauseEvent.WaitOne(Timeout.Infinite);
                Thread.Sleep(rnd.Next(10, 40));
                Invoke(helper, button);
            }
        }

        private void ButtonPause(object sender, EventArgs e)
        {
            buttonStart.Enabled = true;

            //t1.Suspend();
            //t2.Suspend();
            //t3.Suspend();
            Pause(); 

            buttonPause.Enabled = false;
        }

        private void ButtonStop(object sender, EventArgs e)
        {
            ButtonPause(sender, e);

            FullStop();

            ButtonsOnOff(false);
        }

        private void FullStop()
        {
            buttonOne.Location = new Point(27, buttonOne.Location.Y);
            buttonTwo.Location = new Point(27, buttonTwo.Location.Y);
            buttonThree.Location = new Point(27, buttonThree.Location.Y);

            foreach (var item in buttons)
            {
                item.BackColor = SystemColors.Control;
            }
        }

        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            ButtonPause(this, new EventArgs());
        } 
    }
}
