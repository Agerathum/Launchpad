using Gdk;
using Gtk;
using Stetic;
// ReSharper disable CheckNamespace

namespace Launchpad.Utilities.Interface
{
	public sealed partial class MainWindow
	{
		private UIManager UIManager;

		private VBox vbox3;

		private MenuBar menubar1;

		private Alignment alignment14;

		private FileChooserWidget fileChooser;

		private Alignment alignment12;

		private HSeparator hseparator3;

		private HBox hbox4;

		private Alignment alignment18;

		private Label label4;

		private Alignment alignment19;

		private Label progressLabel;

		private Alignment alignment17;

		private ProgressBar progressbar;

		private HBox hbox3;

		private Alignment alignment15;

		private Button generateGameManifestButton;

		private Alignment alignment1;

		private Button generateLaunchpadManifestButton;

		private void Build ()
		{
			Gui.Initialize (this);
			// Widget Launchpad.Utilities.Interface.MainWindow
			this.UIManager = new UIManager ();
			ActionGroup w1 = new ActionGroup ("Default");
			this.UIManager.InsertActionGroup (w1, 0);
			AddAccelGroup (this.UIManager.AccelGroup);
			this.WidthRequest = 640;
			this.HeightRequest = 384;
			this.Name = "Launchpad.Utilities.Interface.MainWindow";
			this.Title = this.LocalizationCatalog.GetString ("Launchpad Utilities - Manifest");
			this.Icon = Pixbuf.LoadFromResource ("Launchpad.Utilities.Resources.Icons8-Android-Industry-Engineering.ico");
			this.WindowPosition = (WindowPosition)4;
			// Container child Launchpad.Utilities.Interface.MainWindow.Gtk.Container+ContainerChild
			this.vbox3 = new VBox
			{
				Name = "vbox3",
				Spacing = 6
			};
			// Container child vbox3.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'/></ui>");
			this.menubar1 = (MenuBar) this.UIManager.GetWidget ("/menubar1");
			this.menubar1.Name = "menubar1";
			this.vbox3.Add (this.menubar1);
			Box.BoxChild w2 = (Box.BoxChild) this.vbox3 [this.menubar1];
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.alignment14 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment14",
				LeftPadding = 8,
				RightPadding = 8
			};
			// Container child alignment14.Gtk.Container+ContainerChild
			this.fileChooser = new FileChooserWidget((FileChooserAction) 2)
			{
				Name = "fileChooser",
				LocalOnly = false
			};
			this.alignment14.Add (this.fileChooser);
			this.vbox3.Add (this.alignment14);
			Box.BoxChild w4 = (Box.BoxChild) this.vbox3 [this.alignment14];
			w4.Position = 1;
			// Container child vbox3.Gtk.Box+BoxChild
			this.alignment12 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment12"
			};
			// Container child alignment12.Gtk.Container+ContainerChild
			this.hseparator3 = new HSeparator
			{
				Name = "hseparator3"
			};
			this.alignment12.Add (this.hseparator3);
			this.vbox3.Add (this.alignment12);
			Box.BoxChild w6 = (Box.BoxChild) this.vbox3 [this.alignment12];
			w6.Position = 2;
			w6.Expand = false;
			w6.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox4 = new HBox
			{
				Name = "hbox4",
				Spacing = 6
			};
			// Container child hbox4.Gtk.Box+BoxChild
			this.alignment18 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment18",
				LeftPadding = 8
			};
			// Container child alignment18.Gtk.Container+ContainerChild
			this.label4 = new Label
			{
				Name = "label4",
				LabelProp = this.LocalizationCatalog.GetString("Progress: ")
			};
			this.alignment18.Add (this.label4);
			this.hbox4.Add (this.alignment18);
			Box.BoxChild w8 = (Box.BoxChild) this.hbox4 [this.alignment18];
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.alignment19 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment19",
				RightPadding = 8
			};
			// Container child alignment19.Gtk.Container+ContainerChild
			this.progressLabel = new Label
			{
				Name = "progressLabel",
				LabelProp = this.LocalizationCatalog.GetString("/some/file/path : 1 out of 100")
			};
			this.alignment19.Add (this.progressLabel);
			this.hbox4.Add (this.alignment19);
			Box.BoxChild w10 = (Box.BoxChild) this.hbox4 [this.alignment19];
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			this.vbox3.Add (this.hbox4);
			Box.BoxChild w11 = (Box.BoxChild) this.vbox3 [this.hbox4];
			w11.Position = 3;
			w11.Expand = false;
			w11.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.alignment17 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment17",
				LeftPadding = 8,
				RightPadding = 8
			};
			// Container child alignment17.Gtk.Container+ContainerChild
			this.progressbar = new ProgressBar
			{
				Name = "progressbar"
			};
			this.alignment17.Add (this.progressbar);
			this.vbox3.Add (this.alignment17);
			Box.BoxChild w13 = (Box.BoxChild) this.vbox3 [this.alignment17];
			w13.Position = 4;
			w13.Expand = false;
			w13.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox3 = new HBox
			{
				Name = "hbox3",
				Spacing = 6
			};
			// Container child hbox3.Gtk.Box+BoxChild
			this.alignment15 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment15",
				RightPadding = 8,
				BottomPadding = 8
			};
			// Container child alignment15.Gtk.Container+ContainerChild
			this.generateGameManifestButton = new Button
			{
				CanFocus = true,
				Name = "generateGameManifestButton",
				UseUnderline = true,
				Label = this.LocalizationCatalog.GetString("Generate Game Manifest")
			};
			this.alignment15.Add (this.generateGameManifestButton);
			this.hbox3.Add (this.alignment15);
			Box.BoxChild w15 = (Box.BoxChild) this.hbox3 [this.alignment15];
			w15.PackType = (PackType)1;
			w15.Position = 0;
			w15.Expand = false;
			w15.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.alignment1 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment1",
				RightPadding = 8,
				BottomPadding = 8
			};
			// Container child alignment1.Gtk.Container+ContainerChild
			this.generateLaunchpadManifestButton = new Button
			{
				CanFocus = true,
				Name = "generateLaunchpadManifestButton",
				UseUnderline = true,
				Label = this.LocalizationCatalog.GetString("Generate Launchpad Manifest")
			};
			this.alignment1.Add (this.generateLaunchpadManifestButton);
			this.hbox3.Add (this.alignment1);
			Box.BoxChild w17 = (Box.BoxChild) this.hbox3 [this.alignment1];
			w17.PackType = (PackType)1;
			w17.Position = 1;
			w17.Expand = false;
			w17.Fill = false;
			this.vbox3.Add (this.hbox3);
			Box.BoxChild w18 = (Box.BoxChild) this.vbox3 [this.hbox3];
			w18.Position = 5;
			w18.Expand = false;
			w18.Fill = false;
			Add (this.vbox3);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}

			this.DefaultWidth = 640;
			this.DefaultHeight = 384;

			Show ();

			this.DeleteEvent += OnDeleteEvent;
			this.generateLaunchpadManifestButton.Clicked += OnGenerateLaunchpadManifestButtonClicked;
			this.generateGameManifestButton.Clicked += OnGenerateGameManifestButtonClicked;
		}
	}
}
