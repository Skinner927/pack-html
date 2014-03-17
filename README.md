# Pack Html 
## An HTML Packer
Html Packer is a tool for merging all script and image dependencies for a given HTML file into the HTML to create a truly portable webpage. 

This is a poor practice in general, but if you have a need or want to only have a single HTML file, this tool is for you.

Pack Html will base64 encode all images and place them inline. Additionally, all JavaScript and CSS will be included directly into the page. CSS files will be scanned for URL tags and those dependencies will be base64 encoded as well.

### No Packing
If you set a `data-no-pack` attribute on an element, that element will not be analyzed for packing.

## Features
- Includes Outside Dependencies
	+ JavaScript
		* via the standard `<script>` tags.
	+ CSS and Inline Styles
		* via the standard `<link>` and `<style>`.
		* base64 encodes all url() mappings (images, fonts, everything).
	+ favicon
		* base64 encodes all icons. Adds missing `<link>` tag if not present and favicon.ico exists.
	+ images
		* via the standard `<img>` tag.
		* base64 encodes all src attributes.
- Ignorable Block
	+ Add the `data-no-pack` attribute to an element and Html Packer will ignore it.

## Download
Look in the `release` folder for compiled binaries. Target platform is Windows 8.1.
