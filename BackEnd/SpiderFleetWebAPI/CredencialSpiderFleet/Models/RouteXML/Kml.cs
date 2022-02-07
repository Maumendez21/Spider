using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CredencialSpiderFleet.Models.RouteXML
{
	[XmlRoot(ElementName = "kml", Namespace = "http://www.opengis.net/kml/2.2")]
	public class Kml
	{
		[XmlElement(ElementName = "Document", Namespace = "http://www.opengis.net/kml/2.2")]
		public Document Document { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
	}

	[XmlRoot(ElementName = "Document", Namespace = "http://www.opengis.net/kml/2.2")]
	public class Document
	{
		[XmlElement(ElementName = "name", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Name { get; set; }
		[XmlElement(ElementName = "description", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Description { get; set; }
		[XmlElement(ElementName = "Style", Namespace = "http://www.opengis.net/kml/2.2")]
		public List<Style> Style { get; set; }
		[XmlElement(ElementName = "StyleMap", Namespace = "http://www.opengis.net/kml/2.2")]
		public List<StyleMap> StyleMap { get; set; }
		[XmlElement(ElementName = "Folder", Namespace = "http://www.opengis.net/kml/2.2")]
		public Folder Folder { get; set; }
	}

	[XmlRoot(ElementName = "Folder", Namespace = "http://www.opengis.net/kml/2.2")]
	public class Folder
	{
		[XmlElement(ElementName = "name", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Placemark", Namespace = "http://www.opengis.net/kml/2.2")]
		public List<Placemark> Placemark { get; set; }
	}

	[XmlRoot(ElementName = "Placemark", Namespace = "http://www.opengis.net/kml/2.2")]
	public class Placemark
	{
		[XmlElement(ElementName = "name", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Name { get; set; }
		[XmlElement(ElementName = "styleUrl", Namespace = "http://www.opengis.net/kml/2.2")]
		public string StyleUrl { get; set; }
		[XmlElement(ElementName = "LineString", Namespace = "http://www.opengis.net/kml/2.2")]
		public LineString LineString { get; set; }
		[XmlElement(ElementName = "Point", Namespace = "http://www.opengis.net/kml/2.2")]
		public Point Point { get; set; }
	}

	[XmlRoot(ElementName = "LineString", Namespace = "http://www.opengis.net/kml/2.2")]
	public class LineString
	{
		[XmlElement(ElementName = "tessellate", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Tessellate { get; set; }
		[XmlElement(ElementName = "coordinates", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Coordinates { get; set; }
	}

	[XmlRoot(ElementName = "Point", Namespace = "http://www.opengis.net/kml/2.2")]
	public class Point
	{
		[XmlElement(ElementName = "coordinates", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Coordinates { get; set; }
	}

	[XmlRoot(ElementName = "StyleMap", Namespace = "http://www.opengis.net/kml/2.2")]
	public class StyleMap
	{
		[XmlElement(ElementName = "Pair", Namespace = "http://www.opengis.net/kml/2.2")]
		public List<Pair> Pair { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}

	[XmlRoot(ElementName = "Pair", Namespace = "http://www.opengis.net/kml/2.2")]
	public class Pair
	{
		[XmlElement(ElementName = "key", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Key { get; set; }
		[XmlElement(ElementName = "styleUrl", Namespace = "http://www.opengis.net/kml/2.2")]
		public string StyleUrl { get; set; }
	}

	[XmlRoot(ElementName = "Style", Namespace = "http://www.opengis.net/kml/2.2")]
	public class Style
	{
		[XmlElement(ElementName = "IconStyle", Namespace = "http://www.opengis.net/kml/2.2")]
		public IconStyle IconStyle { get; set; }
		[XmlElement(ElementName = "LabelStyle", Namespace = "http://www.opengis.net/kml/2.2")]
		public LabelStyle LabelStyle { get; set; }
		[XmlElement(ElementName = "BalloonStyle", Namespace = "http://www.opengis.net/kml/2.2")]
		public BalloonStyle BalloonStyle { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "LineStyle", Namespace = "http://www.opengis.net/kml/2.2")]
		public LineStyle LineStyle { get; set; }
	}

	[XmlRoot(ElementName = "IconStyle", Namespace = "http://www.opengis.net/kml/2.2")]
	public class IconStyle
	{
		[XmlElement(ElementName = "color", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Color { get; set; }
		[XmlElement(ElementName = "scale", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Scale { get; set; }
		[XmlElement(ElementName = "Icon", Namespace = "http://www.opengis.net/kml/2.2")]
		public Icon Icon { get; set; }
		[XmlElement(ElementName = "hotSpot", Namespace = "http://www.opengis.net/kml/2.2")]
		public HotSpot HotSpot { get; set; }
	}

	[XmlRoot(ElementName = "LabelStyle", Namespace = "http://www.opengis.net/kml/2.2")]
	public class LabelStyle
	{
		[XmlElement(ElementName = "scale", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Scale { get; set; }
	}

	[XmlRoot(ElementName = "BalloonStyle", Namespace = "http://www.opengis.net/kml/2.2")]
	public class BalloonStyle
	{
		[XmlElement(ElementName = "text", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Icon", Namespace = "http://www.opengis.net/kml/2.2")]
	public class Icon
	{
		[XmlElement(ElementName = "href", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Href { get; set; }
	}

	[XmlRoot(ElementName = "hotSpot", Namespace = "http://www.opengis.net/kml/2.2")]
	public class HotSpot
	{
		[XmlAttribute(AttributeName = "x")]
		public string X { get; set; }
		[XmlAttribute(AttributeName = "xunits")]
		public string Xunits { get; set; }
		[XmlAttribute(AttributeName = "y")]
		public string Y { get; set; }
		[XmlAttribute(AttributeName = "yunits")]
		public string Yunits { get; set; }
	}

	[XmlRoot(ElementName = "LineStyle", Namespace = "http://www.opengis.net/kml/2.2")]
	public class LineStyle
	{
		[XmlElement(ElementName = "color", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Color { get; set; }
		[XmlElement(ElementName = "width", Namespace = "http://www.opengis.net/kml/2.2")]
		public string Width { get; set; }
	}
}