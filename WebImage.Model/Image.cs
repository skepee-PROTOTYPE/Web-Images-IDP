using MyAzure;
using System;
using System.Text;
using System.Threading.Tasks;

namespace WebImage.Model
{
    public class Image
    {
        public int ImageId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string RawFormat { get; set; }
        public double? Length { get; set; }
        public int CategoryId { get; set; }
        public bool IsPrivate { get; set; }
        public bool? IsLandscape { get; set; }
        public string PixelFormat { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public double? HorizontalResolution { get; set; }
        public double? VerticalResolution { get; set; }
        public string ThumbFileName { get; set; }
        public string ResizedFileName { get; set; }
        public string UserId { get; set; }
        public string Url { get; set; }
        public string UrlResized { get; set; }
        public string UrlThumb { get; set; }
        public byte[]? Content { get; set; }
        public string Description { get; set; }

        public Image HideAttributes(string hidecolumns)
        {
            //this.ImageId = 0;
            this.UserId = null;
            this.Url = hidecolumns.Contains("url") ? "" : this.Url;
            this.Name = hidecolumns.Contains("name") ? "" : this.Name;
            this.Title = hidecolumns.Contains("title") ? "" : this.Title;
            this.RawFormat = hidecolumns.Contains("format") ? "" : this.RawFormat;
            this.Length = hidecolumns.Contains("length") ? 0 : this.Length;
            this.IsLandscape = (hidecolumns.Contains("landscape") ? null : this.IsLandscape);
            this.PixelFormat = hidecolumns.Contains("pixel") ? "" : this.PixelFormat;
            this.Width = hidecolumns.Contains("size") ? 0 : this.Width;
            this.Height = hidecolumns.Contains("size") ? 0 : this.Height;
            this.HorizontalResolution = hidecolumns.Contains("resolution") ? 0 : this.HorizontalResolution;
            this.VerticalResolution = hidecolumns.Contains("resolution") ? 0 : this.VerticalResolution;
            this.Content = hidecolumns.Contains("content") ? null : this.Content;

            return this;
        }

    }

}
