using System;
using System.Numerics;
using Avalonia.OpenGL;
using FreeTypeSharp;
using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.backends;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.controls.gl;
using SomeChartsUiAvalonia.utils;
using SomeChartsUiAvalonia.utils.collections;

namespace SomeChartsUiAvalonia.backends; 

public class GlChartsBackend : ChartsBackendBase {
	public static bool perspectiveMode = false;

	private Font? testFont;

	//TODO: oof
	public override unsafe void DrawText(string text, color col, FontData font, RenderableTransform transform) {
		testFont ??= Font.LoadFromPath("data/Comfortaa-VariableFont_wght.ttf", renderer.owner);
		// (FontCharData ch, int atlas) glyph = "AMOGUSamogus".ToCharArray().Select(c => testFont.textures.GetGlyph(c.ToString())).Last();
		Random rnd = new();
		//char c = (char) rnd.Next(60, 200);
		//c = 'A';
		//(FontCharData ch, int atlas) glyph = testFont.textures.GetGlyph(c.ToString());
		// for (int i = 0; i < 64; i++)
		// {
		// 	char c = (char) rnd.Next(60, 2000);
		// 	testFont.textures.GetGlyph(c.ToString());
		// }
		//foreach (char c in "AMOGUSamogus")
		//	testFont.textures.GetGlyph(c.ToString());

		//Console.WriteLine($"{glyph.atlas} :: {glyph.ch}");

		// Vertex[] vertices = new Vertex[4] {
		// 	new(new(0, 0), new(0, 0), new(glyph.ch.position.x / 1024, glyph.ch.position.y / 1024), color.white),
		// 	new(new(0, 1000), new(0, 1), new(glyph.ch.position.x / 1024, (glyph.ch.position.y + glyph.ch.size.y) / 1024), color.white),
		// 	new(new(1000, 1000), new(1, 1), new((glyph.ch.position.x + glyph.ch.size.x)/ 1024, (glyph.ch.position.y + glyph.ch.size.y) / 1024), color.white),
		// 	new(new(1000, 0), new(1, 0), new((glyph.ch.position.x + glyph.ch.size.x) / 1024, glyph.ch.position.y / 1024), color.white),
		// };
		//color col = "#FF8855";
		
		
		ushort[] indices = {0, 1, 2, 0, 2, 3};



		for (int i = 0; i < 1; i++) {
			float xPos = 0;
			float zPos = i * 0.1f;
			foreach (char c in "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed tincidunt mollis pretium. Phasellus euismod tortor consequat, lobortis erat quis, accumsan ligula. Vestibulum at dolor congue ante ultricies lacinia. Vestibulum nec tellus nec mauris porttitor rhoncus. Nulla pellentesque placerat pulvinar. Nullam ut rutrum ante. Nulla nunc tellus, accumsan et facilisis ut, placerat vel neque. Proin feugiat eleifend justo nec posuere. Vestibulum convallis ipsum ut rutrum faucibus. Fusce faucibus suscipit nibh, ac pellentesque lorem pellentesque et. Donec at arcu a purus ultrices sagittis. Proin at congue diam.") {
				(FontCharData ch, int atlas) glyph = testFont.textures.GetGlyph(c.ToString());
			
				float s = 16 / 32f;
				Vertex[] vertices = new Vertex[4] {
					new(new(xPos, 0, zPos), new(0, 0), new(glyph.ch.position.x / 1024, glyph.ch.position.y / 1024 + glyph.ch.size.y / 1024), col),
					new(new(xPos, s*glyph.ch.size.y, zPos), new(0, 1), new(glyph.ch.position.x / 1024, glyph.ch.position.y / 1024), col),
					new(new(xPos+s*glyph.ch.size.x, s*glyph.ch.size.y, zPos), new(1, 1), new(glyph.ch.position.x / 1024 + glyph.ch.size.x / 1024, glyph.ch.position.y / 1024), col),
					new(new(xPos+s*glyph.ch.size.x, 0, zPos), new(1, 0), new(glyph.ch.position.x / 1024 + glyph.ch.size.x / 1024, glyph.ch.position.y / 1024 + glyph.ch.size.y / 1024), col),
				};
			
				using Mesh m = owner.factory.CreateMesh();
				m.SetIndexes(indices);
				m.SetVertices(vertices);
		
				if (testFont.textures.atlases.Count > 0) {
					((GlTexture)testFont.textures.atlases[0].texture).Bind();
					Material mat = new(GlShaders.basicText);
					mat.SetProperty("texture0", ((GlTexture)testFont.textures.atlases[0].texture));
					DrawMesh(m, mat, transform);
				}

				xPos += 16;
			}
		}

		
	}
	
	public override void ClearScreen(color col) {
		GlInfo.gl!.ClearColor(col.rF, col.gF, col.bF, col.aF);
		GlInfo.gl.Clear(GlConsts.GL_COLOR_BUFFER_BIT | GlConsts.GL_DEPTH_BUFFER_BIT);
	}

	public override void DrawMesh(Mesh mesh, Material? material, RenderableTransform transform) {
		if (mesh is not GlMesh obj) throw new NotImplementedException("opengl backend support only GlMesh mesh type; use CreateMesh() in ChartsBackendBase");

		float z = 1 / owner.transform.zoom.animatedValue.x;
		Matrix4x4 projection = Matrix4x4.CreateOrthographic(owner.transform.screenBounds.width*z, owner.transform.screenBounds.height*z, .001f, 10000);
		//TODO: fix perspective
		if (perspectiveMode) {
			projection = Matrix4x4.CreatePerspectiveFieldOfView(90 / 180f * MathF.PI, (owner.transform.screenBounds.width / owner.transform.screenBounds.height), .001f, 10000f);
			z *= 100;
		}


		float3 camPos = owner.transform.position.animatedValue;
		Matrix4x4 view = Matrix4x4.CreateLookAt(new(-camPos.x, camPos.y, z), new(-camPos.x,camPos.y,0), new(0, -1, 0));

		float3 p = transform.position;
		Matrix4x4 model = Matrix4x4.CreateFromYawPitchRoll(transform.rotation.x, transform.rotation.y, transform.rotation.z) * Matrix4x4.CreateScale(new Vector3(-transform.scale.x, transform.scale.y, transform.scale.z)) * Matrix4x4.CreateTranslation(new(-p.x, -p.y, p.z));
		
		obj.Render(material, model, view, projection, camPos);
	}

	
	
	
}