using MudBlazor;

namespace EvaWoods.Web.Theme;

public class EvaWoodsTheme : MudTheme
{
    public EvaWoodsTheme()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2F5233",
            Secondary = "#C9A227",
            Background = "#FAF7F2",
            Surface = "#FFFFFF",
            AppbarBackground = "#FFFFFF",
            AppbarText = "#3A3A3A",
            DrawerBackground = "#FFFFFF",
            DrawerText = "#3A3A3A",
            Success = "#2F9E44",
            Warning = "#E8A33D",
            Error = "#D64545"
        };

        LayoutProperties = new LayoutProperties
        {
            DrawerWidthLeft = "260px",
            DefaultBorderRadius = "8px"
        };
    }
}