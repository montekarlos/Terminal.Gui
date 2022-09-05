namespace StandaloneExample {

	using Terminal.Gui;
	using System;

	using System.Threading.Tasks;
	using System.Collections.Generic;

	static class Demo {

		public static Label debugText1;
        public static Label debugText2;
        public static Label ml;

		static Random _random = new Random();
        static async Task Launch(Int32 i)
		{
			await Task.Delay(_random.Next(200, 400));
			Application.MainLoop.Invoke(() => debugText2.Text = $"index{i}");
        }

		static async Task StartBackgroundUpdate()
		{
			while (true)
			{
				await Task.Delay(3000); // Wait for ui to start
                Application.MainLoop.Invoke(() => debugText1.Text = $"Starting cycle...");

				List<Task> list = new List<Task>();
                for (var i = 0; i < 10000; i++)
                {
                    list.Add(Launch(i));
                }

                Application.MainLoop.Invoke(() => debugText1.Text = $"Waiting for task completion...");

                foreach (var task in list)
				{
					await task;	// Sometimes doesn't resume
				}
                Application.MainLoop.Invoke(() => debugText1.Text = $"Completed cycle OK!");
            }
		}

		static void Main (string [] args)
		{
			Application.Init ();

			var win = new Window ("Hello") {
				X = 0,
				Y = 1,
				Width = Dim.Fill (),
				Height = Dim.Fill () - 1
			};

			int count = 0;
			ml = new Label (new Rect (0, 0, 47, 1), "Mouse: ");
			debugText1 = new Label("....") { X = 8, Y = 1 };
            debugText2 = new Label("....") { X = 8, Y = 2 };
            Application.RootMouseEvent += (me) => ml.Text = $"Mouse: ({me.X},{me.Y}) - {me.Flags} {count++}";

			win.Add (ml, 
					 new Label("Debug1: ") { X = 0, Y = 1 }, debugText1,
                     new Label("Debug2: ") { X = 0, Y = 2 }, debugText2);

			_ = StartBackgroundUpdate();

			Application.Top.Add (win);
			Application.Run ();

			Application.Shutdown ();
		}
	}
}