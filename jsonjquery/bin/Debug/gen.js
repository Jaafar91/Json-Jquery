$(document).ready(function(){

	$("#fname").prop('required',"true");
	$("#lname").prop('required',"true");
	$("#age").prop('required',"true");
	$(".help-block").hide();

	$('#btn1').on('click',function(e){
		callMyFunction();
		$(".help-block").hide();
		$(this).show();
		$(this).css('background-color',"blue");
		$(this).css('font-size',"20px");
		$("#fname").css('color',"yellow");
		$(".help-block").css('position',"absolute");
		$("p").addClass("text-center");
		$(".help-block").append("Hello");
	});

	$('#btn1').on('mousemove',function(e){
		$(this).css('background-color',"red");
	});

	$('#btn1').on('mouseleave',function(e){
		$(this).css('background-color',"grey");
		$(this).attr('style',"color:#005670;");
	});

	$('#btn2').on('click',function(e){
		$(".help-block").show(3000);
	});

	$('#btn3').on('click',function(e){
		$(this).fadeTo('slow',0.15);
		$(".help-block").animate({
  "left": "250px",
  "opacity": "0.5",
  "height": "150px",
  "width": "150px"
});
	});

	$('#btn4').on('click',function(e){
	});

		$.get('http://localhost:6352/api/user/count',function(response){});
		
});
