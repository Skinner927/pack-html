var ballBounce = function() {
	var context;
	var x=100;
	var y=200;
	var dx=3;
	var dy=5;

	function init()
	{
	  context= myCanvas.getContext('2d');
	  setInterval(draw,10);
	}

	function draw()
	{
	  context.clearRect(0,0, 300,300);
	  context.beginPath();
	  context.fillStyle="#0000ff";
	  // Draws a circle of radius 20 at the coordinates 100,100 on the canvas
	  context.arc(x,y,20,0,Math.PI*2,true);
	  context.closePath();
	  context.fill();
	  // Boundary Logic
		if( x<0 || x>300) dx=-dx; 
		if( y<0 || y>300) dy=-dy; 
		x+=dx; 
		y+=dy;
	}
	
	// start it
	init();
}