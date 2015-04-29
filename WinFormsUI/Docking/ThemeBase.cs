using System.ComponentModel;

namespace TickDream.WinUI.Docking
{
	public abstract class ThemeBase : Component, ITheme
	{
	    public abstract void Apply(DockPanel dockPanel);
	}
}
