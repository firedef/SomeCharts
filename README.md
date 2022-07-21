# SomeCharts
SomeCharts is a C# cross-platform library for data visualization.

<img src="https://user-images.githubusercontent.com/57886233/158441005-002cd107-ed15-4c9b-84eb-82ec95199c7c.png" width="400" />

# Features
- early version by now
- support only Avalonia by now. Only immediate renderer supported
- cross-platform (tested only on Linux, but should works on win, mac and (maybe) on mobile devices)
- OpenGl backend, planned to add Skia
- glsl shaders, materials, post-processing, 3D mesh with normals and uvs
- fast custom (WIP) text rendering (millions sumbols at 60 fps)
- dynamic data update (customizable)
- levels of detail, occlusion culling
- physical and rendering layers
- very fast (MUCH faster then analogs)
- (0,0) is bottom-left corner. No inverted Y-axis!
- flexible data. Support arrays, collections, functions and constants as data source
- very fast OpenGl textures (much faster then in Skia)
- mesh updates only when necessary
- debug functions by pressing keys
- customizable bloom and fxaa
- themes

# Charts that already implemented
- ruler (grid with customizable size, bounds and labels)
- line (any length)
<img src="https://user-images.githubusercontent.com/57886233/158448462-e0760960-2c86-4f5f-a6ed-37e92d42d533.png" width="400" />
- pie (donut-shape with labels)
<img src="https://user-images.githubusercontent.com/57886233/158448972-6b13e88d-c59b-4a05-b584-5077da12c4c2.png" width="400" />
- scatter (points with various positions, sizes, colors and shapes)
<img src="https://user-images.githubusercontent.com/57886233/158449190-3c794a90-caae-43a5-be32-d041de0da38f.png" width="400" />

# Custom text rendering
- based on FreeType
- full unicode
- fallback fonts
- custom SDF shader with subpixel antialiasing. Good quality at large scale and ok at small scale
- very fast. Can rebuild mesh of 2.7mb text file in ~5 sec and render at 60 fps

<img src="https://user-images.githubusercontent.com/57886233/158447362-f1e19c6f-658a-4682-8a15-9c1937fd58f1.png" width="400" /><img src="https://user-images.githubusercontent.com/57886233/158447565-809d3fe6-224c-46ac-9560-8ac97ba39e5e.png" width="400" />

# Shortcuts
- [T] - switch teme (light/dark)
- [Y] - switch polygon mode (render as wireframe or points)
- [U] - toggle materials
- [L] - change text quality
- [K] - toggle post processing
- [O], [shift]+[O] - change text thickness
- [W], [A], [S], [D] - movement
- [Alt] - faster mouse scroll and pan

# Charts that need to port (written in old version)
- heatmap
- shapefile map
- candlesticks
- bar

# Code
full examples can be found [there](SomeChartsAvaloniaExamples/src/elements/)

🔻 deffered renderer (avalonia) is broken! use immediate renderer
```C#
// get canvas
AvaloniaGlChartsCanvas canvas = this.FindControl<AvaloniaGlChartsCanvas>("ControlName");

// add grid
canvas.AddRuler(Orientation.horizontal, 1_000_000);
canvas.AddRuler(Orientation.vertical, 1_000_000);

// add line chart
IChartData<float> data = new FuncChartData<float>(j => j * 5, lineLength);
IChartData<indexedColor> colors = new ConstChartData<indexedColor>(theme.accent0_ind);
canvas.AddLineChart(data, colors);
```
