﻿//
//  Launchpad.Launcher.Interface.MainWindow.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using Gdk;
using Gtk;
using Launchpad.Common;
using Stetic;
using Image = Gtk.Image;

namespace Launchpad.Launcher.Interface
{
	public sealed partial class MainWindow
	{
		private UIManager UIManager;

		private Action MenuAction;

		private Action RepairGameAction;
		private Action ReinstallGameAction;

		private VBox VBox1;

		private MenuBar MainMenuBar;

		private HBox HBox2;

		private VBox BrowserContainer;

		private Alignment Alignment2;

		private ScrolledWindow BrowserWindow;

		private Alignment Alignment5;

		private Image GameBanner;

		private Alignment Alignment1;

		private Label IndicatorLabel;

		private HBox HBox3;

		private Alignment Alignment4;

		private ProgressBar MainProgressBar;

		private HBox HBox4;

		private Alignment Alignment3;

		private Button PrimaryButton;

		private void Build()
		{
			Gui.Initialize(this);

			// Widget Launchpad.Launcher.Interface.MainWindow
			this.UIManager = new UIManager();
			var mainActionGroup = new ActionGroup("Default");

			this.MenuAction = new Action("MenuAction", LocalizationCatalog.GetString("Menu"), null, null)
			{
				ShortLabel = LocalizationCatalog.GetString("Menu")
			};
			mainActionGroup.Add(this.MenuAction, null);

			this.RepairGameAction = new Action(
				"repairGameAction",
				LocalizationCatalog.GetString("Repair Game"),
				LocalizationCatalog.GetString("Starts a repair process for the installed game."),
				"gtk-refresh")
			{
				ShortLabel = LocalizationCatalog.GetString("Repair Game")
			};
			mainActionGroup.Add(this.RepairGameAction, null);

			this.ReinstallGameAction = new Action(
				"reinstallGameAction",
				LocalizationCatalog.GetString("Reinstall Game"),
				LocalizationCatalog.GetString("Reinstalls the installed game."),
				"gtk-refresh")
			{
				ShortLabel = LocalizationCatalog.GetString("Reinstall Game")
			};
			mainActionGroup.Add(this.ReinstallGameAction, null);

			this.UIManager.InsertActionGroup(mainActionGroup, 0);
			AddAccelGroup(this.UIManager.AccelGroup);
			this.Name = "Launchpad.Launcher.Interface.MainWindow";
			this.Title = LocalizationCatalog.GetString("Launchpad - {0}");

			if (SystemInformation.IsRunningOnUnix())
			{
				this.Icon = Pixbuf.LoadFromResource("Launchpad.Launcher.Resources.RocketIcon.ico");
			}
			else
			{
				this.Icon = Pixbuf.LoadFromResource("Launchpad.Launcher.Resources.RocketIcon_Grey.ico");
			}

			this.WindowPosition = (WindowPosition)4;
			this.DefaultWidth = 745;
			this.DefaultHeight = 415;
			this.Resizable = false;

			// Container child Launchpad.Launcher.Interface.MainWindow.Gtk.Container+ContainerChild
			this.VBox1 = new VBox
			{
				Name = "VBox1",
				Spacing = 6
			};

			// Container child VBox1.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString
			(
				"<ui>" +
				"<menubar name='MainMenuBar'>" +
				"<menu name='MenuAction' action='MenuAction'>" +
				"<menuitem name='repairGameAction' action='repairGameAction'/>" +
				"<separator/>" +
				"<menuitem name='reinstallGameAction' action='reinstallGameAction'/>" +
				"</menu>" +
				"</menubar>" +
				"</ui>"
			);
			this.MainMenuBar = (MenuBar)this.UIManager.GetWidget("/MainMenuBar");
			this.MainMenuBar.Name = "MainMenuBar";
			this.VBox1.Add(this.MainMenuBar);

			var w2 = (Box.BoxChild)this.VBox1[this.MainMenuBar];
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;

			// Container child VBox1.Gtk.Box+BoxChild
			this.HBox2 = new HBox
			{
				Name = "HBox2",
				Spacing = 6,
				BorderWidth = 4
			};

			// Container child HBox2.Gtk.Box+BoxChild
			this.BrowserContainer = new VBox
			{
				Name = "browserContainer",
				Spacing = 6
			};

			// Container child browserContainer.Gtk.Box+BoxChild
			this.Alignment2 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				WidthRequest = 310,
				Name = "alignment2"
			};

			// Container child alignment2.Gtk.Container+ContainerChild
			this.BrowserWindow = new ScrolledWindow
			{
				CanFocus = true,
				Name = "browserWindow",
				ShadowType = ShadowType.In
			};
			this.Alignment2.Add(this.BrowserWindow);
			this.BrowserContainer.Add(this.Alignment2);

			var w4 = (Box.BoxChild)this.BrowserContainer[this.Alignment2];
			w4.Position = 0;
			this.HBox2.Add(this.BrowserContainer);

			var w5 = (Box.BoxChild)this.HBox2[this.BrowserContainer];
			w5.Position = 0;
			w5.Expand = false;

			// Container child HBox2.Gtk.Box+BoxChild
			this.Alignment5 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment5"
			};

			// Container child alignment5.Gtk.Container+ContainerChild
			this.GameBanner = new Image
			{
				WidthRequest = 450,
				HeightRequest = 300,
				Name = "gameBanner",
				Pixbuf = Pixbuf.LoadFromResource("Launchpad.Launcher.Resources.RocketIcon.ico")
			};
			this.Alignment5.Add(this.GameBanner);
			this.HBox2.Add(this.Alignment5);

			var w7 = (Box.BoxChild)this.HBox2[this.Alignment5];
			w7.Position = 1;
			this.VBox1.Add(this.HBox2);

			var w8 = (Box.BoxChild)this.VBox1[this.HBox2];
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;

			// Container child VBox1.Gtk.Box+BoxChild
			this.Alignment1 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment1",
				LeftPadding = 6
			};

			// Container child alignment1.Gtk.Container+ContainerChild
			this.IndicatorLabel = new Label
			{
				Name = "IndicatorLabel",
				Xalign = 0F,
				LabelProp = LocalizationCatalog.GetString("Idle")
			};
			this.Alignment1.Add(this.IndicatorLabel);
			this.VBox1.Add(this.Alignment1);

			var w10 = (Box.BoxChild)this.VBox1[this.Alignment1];
			w10.Position = 2;
			w10.Expand = false;
			w10.Fill = false;

			// Container child VBox1.Gtk.Box+BoxChild
			this.HBox3 = new HBox
			{
				Name = "HBox3",
				Spacing = 6,
				BorderWidth = 4
			};

			// Container child HBox3.Gtk.Box+BoxChild
			this.Alignment4 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				Name = "alignment4"
			};

			// Container child alignment4.Gtk.Container+ContainerChild
			this.MainProgressBar = new ProgressBar
			{
				Name = "mainProgressBar"
			};
			this.Alignment4.Add(this.MainProgressBar);
			this.HBox3.Add(this.Alignment4);

			var w12 = (Box.BoxChild)this.HBox3[this.Alignment4];
			w12.Position = 0;

			// Container child HBox3.Gtk.Box+BoxChild
			this.HBox4 = new HBox
			{
				Name = "HBox4",
				Spacing = 6
			};

			// Container child HBox4.Gtk.Box+BoxChild
			this.Alignment3 = new Alignment(0.5F, 0.5F, 1F, 1F)
			{
				WidthRequest = 100,
				Name = "alignment3"
			};

			// Container child alignment3.Gtk.Container+ContainerChild
			this.PrimaryButton = new Button
			{
				Sensitive = false,
				CanDefault = true,
				CanFocus = true,
				Name = "primaryButton",
				UseUnderline = true,
				Label = LocalizationCatalog.GetString("Inactive")
			};
			this.Alignment3.Add(this.PrimaryButton);
			this.HBox4.Add(this.Alignment3);

			var w14 = (Box.BoxChild)this.HBox4[this.Alignment3];
			w14.Position = 0;
			this.HBox3.Add(this.HBox4);

			var w15 = (Box.BoxChild)this.HBox3[this.HBox4];
			w15.Position = 1;
			w15.Expand = false;
			w15.Fill = false;
			this.VBox1.Add(this.HBox3);

			var w16 = (Box.BoxChild)this.VBox1[this.HBox3];
			w16.Position = 3;
			w16.Expand = true;
			w16.Fill = false;

			Add(this.VBox1);
			this.Child?.ShowAll();

			this.PrimaryButton.HasDefault = true;
			Show();
			this.DeleteEvent += OnDeleteEvent;
			this.RepairGameAction.Activated += OnRepairGameActionActivated;
			this.ReinstallGameAction.Activated += OnReinstallGameActionActivated;
			this.PrimaryButton.Clicked += OnPrimaryButtonClicked;
		}
	}
}
