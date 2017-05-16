$(document).ready(function(){
	drawCalendarInDiv($('#divCalendar'));
	setInterval(
		function (){ 
			drawCounter()
	}, 1);
});

function drawCalendarInDiv(elementToDraw)
{
	$(elementToDraw).fullCalendar({
		dayRender: function( date, cell ) {
			var actualCalendarDate = date.format();
			
			if(isDateBeforeToday(actualCalendarDate)){
				cell.addClass('passedDay');
				return;
			}
			
			if(isFutureDay(actualCalendarDate) && isDateBeforeLimit(actualCalendarDate)){
				cell.addClass('notPassedDay');
				return;
			}
		}
	});
}

function isDateBeforeToday(dateToCompare)
{
	var date1 = new Date(dateToCompare);
	var date2 = new Date(getTodayDate());
	
	return date1 < date2;
}

function isDateBeforeLimit(dateToCompare)
{
	var date1 = new Date(dateToCompare);
	var date2 = new Date('2017-07-20');
	
	return date1 < date2;
}

function isFutureDay(dateToCompare)
{
	var date1 = new Date(dateToCompare);
	var date2 = new Date(getTodayDate());
	
	return date1 > date2;
}

function getTodayDate()
{
	var today = new Date();
	var dd = today.getDate();
	var mm = today.getMonth()+1; //January is 0!
	var yyyy = today.getFullYear();
	var HH = today.getHours();
	var MM = today.getMinutes();
	var ss = today.getSeconds();
	var ms = today.getMilliseconds();

	if(dd<10) {
		dd='0'+dd
	} 

	if(mm<10) {
		mm='0'+mm
	} 

	today = yyyy + '-' + mm + '-' + dd + ' ' + HH + ':' + MM + ':' + ss + ':' + ms;
	return today;
}

function drawCounter()
{
	var today = new Date(getTodayDate());
	var limitDate = new Date('2017-07-20 00:00:000');
	var difference = limitDate - today;
	
	var days = parseInt((difference / (1000*60*60*24)));
	var hours = parseInt((difference / (60 * 60 * 1000)));
	var mins = parseInt((difference / (60 * 1000)) % 60);
	var secs = parseInt((difference / 1000) % 60);
	
	$('#days').text(days + ' Days');
	$('#hours').text(hours + ' Hours');
	$('#minutes').text(mins + ' Minutes');
	$('#seconds').text(secs + ' Seconds');
}