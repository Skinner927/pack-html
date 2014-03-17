# Pack Html 
## An HTML Packer
Html Packer is a tool for merging all script and image depenencies for a given HTML file into the HTML to create a truly portable webpage. 

This is a poor practice in general, but if you have a need or want to only have a single HTML file, this tool is for you.

Pack Html will base64 encode all images and place them inline. Additionally, all Javascript and CSS will be included directly into the page. CSS files will be scanned for URL tags and those dependencies will be base64 encoded as well.

### To Do
+ Parsers
	- CSS
		* Init
		* Parse URLs
	- JavaScript
