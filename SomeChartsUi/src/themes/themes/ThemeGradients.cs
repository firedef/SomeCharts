using SomeChartsUi.themes.colors;

namespace SomeChartsUi.themes.themes;

public partial record theme {
    public Gradient whiteGradient = new((0, white_ind));
    public Gradient blackGradient = new((0, black_ind));
    
    public Gradient grayscaleGradient = new(default0_ind, default2_ind, default4_ind, default6_ind, default8_ind, default10_ind);
    
    public Gradient goodGradient = Gradient.FromColor(good_ind);
    public Gradient normalGradient = Gradient.FromColor(normal_ind);
    public Gradient badGradient = Gradient.FromColor(bad_ind);
    
    public Gradient accent0Gradient = Gradient.FromColor(accent0_ind);
    public Gradient accent1Gradient = Gradient.FromColor(accent1_ind);
    public Gradient accent2Gradient = Gradient.FromColor(accent2_ind);
}