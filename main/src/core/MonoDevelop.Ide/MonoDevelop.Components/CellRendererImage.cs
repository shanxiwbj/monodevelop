//
// CellRendererImage.cs
//
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
//
// Copyright (c) 2013 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using Xwt.Drawing;

namespace MonoDevelop.Components
{
	public class CellRendererImage: Gtk.CellRenderer
	{
		Image image;
		Image imageOpen;
		Image imageClosed;

		public CellRendererImage ()
		{
		}

		[GLib.Property ("image")]
		public virtual Image Image {
			get {
				return image;
			}
			set {
				image = value;
			}
		}
		
		[GLib.Property ("image-expander-open")]
		public virtual Image ImageExpanderOpen {
			get {
				return imageOpen;
			}
			set {
				imageOpen = value;
			}
		}

		[GLib.Property ("image-expander-closed")]
		public virtual Image ImageExpanderClosed {
			get {
				return imageClosed;
			}
			set {
				imageClosed = value;
			}
		}

		protected override void Render (Gdk.Drawable window, Gtk.Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, Gtk.CellRendererState flags)
		{
			if (image == null)
				return;

			using (var ctx = Gdk.CairoHelper.Create (window)) {
				var img = IsExpanded ? (imageOpen ?? image) : (imageClosed ?? image);
				var x = Xpad + cell_area.X + cell_area.Width / 2 - (int)(img.Width / 2);
				var y = Ypad + cell_area.Y + cell_area.Height / 2 - (int)(img.Height / 2);
				ctx.DrawImage (widget, img, x, y);
			}
		}

		protected void GetImageInfo (Gdk.Rectangle cell_area, out Image img, out int x, out int y)
		{
			img = IsExpanded ? (imageOpen ?? image) : (imageClosed ?? image);
			x = (int)(Xpad + cell_area.X + cell_area.Width / 2 - (int)(img.Width / 2));
			y = (int)(Ypad + cell_area.Y + cell_area.Height / 2 - (int)(img.Height / 2));
		}

		public override void GetSize (Gtk.Widget widget, ref Gdk.Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)
		{
			if (image != null) {
				width = (int)image.Width;
				height = (int)image.Height;
			} else
				width = height = 0;

			x_offset = y_offset = 0;
		}
	}
}

