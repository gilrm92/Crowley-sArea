var _hour;
var _minute;
var _url;
var _intervalID = 0;

$(document).ready(function(){
	setInterval(function() { displayTime(); }, 500);
});

function saveInfo()
{
	_hour = $('#hour').val();
	_minute = $('#minute').val();
	_url = $('#url').val();
	
	setDisplaySpans();
	
	if(_hour != "" && _minute != "" && _url != "")
	{
		if(_intervalID != 0)
		{
			clearInterval(_intervalID);
		}
		
		_intervalID = setInterval(function() { verifyAlarm(); }, 1000);
	}
	
}

function setDisplaySpans()
{
	$("#alarmTime").text(_hour + ":" + _minute);
	$("#urlDisplay").text(_url);
}

function displayTime(){
	
	var now = getHour();
	$("#now").text(now);
}

function verifyAlarm()
{
	var now = new Date();
	
	if(_hour == now.getHours() && _minute == now.getMinutes())
	{
		window.open(_url);
		clearInterval(_intervalID);
	}
}

function getHour()
{
	var today = new Date();
	var HH = today.getHours();
	var MM = today.getMinutes();

	hour = HH + ':' + MM;
	return hour;
}