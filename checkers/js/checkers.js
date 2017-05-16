var _color = "";
var _name = "";
var _email = "";
var _signalrConnection;  
var _checkersHub;  
var _loadOpponentInterval;
var _isPlayerOne = false;
var _isPlayerTwo = false;

$(document).ready(function(){
	//drawTable();
	//setBlackPieces();
	
	//setWhitePieces();
	connect();
	
});

function connect() {  
    var checkersServerURL = "http://crowleysarea.somee.com/checkers/admin/CheckersHost/publish/";  
    var checkersHubURL = checkersServerURL + "signalr";  

    _signalrConnection = $.hubConnection(checkersHubURL, {  
        useDefaultPath: false  
    });
	
    _checkersHub = _signalrConnection.createHubProxy('CheckersHub'); 
	_checkersHub.on("getAvaiableOpponentsCallback", function(avaiablePlayers)
	{
		displayOnTable(avaiablePlayers);
	});
	
	_checkersHub.on("checkSessionCallback", function(player) 
	{
		if(player != undefined && player != null)
		{
			userAlreadyLoggedIn(player);
		}
	});
	
    _signalrConnection.start().done(function(){ checkSession();});
}

function login()
{
	_name = $('#name').val();
	_email = $('#email').val();
	
	if(_name == "" && _email == "")
	{
		alert("Please, fill this shit fields!");
		return;
	}
	
	drawTable();
	$('#table').show();
	$('#play').show();
	$('#init').hide();
	
	registerPlayer();
	alert("Welcome " + _name);
	
	_loadOpponentInterval = setInterval(function() {loadOpponents();}, 5000);
}

function checkSession()
{
	_checkersHub.invoke('CheckSession');
}

function userAlreadyLoggedIn(player)
{
	_name = player.Name;
	_email = player.Email;
	
	drawTable();
	$('#table').show();
	$('#play').show();
	$('#init').hide();
	
	_loadOpponentInterval = setInterval(function() {loadOpponents();}, 5000);
}

function loadOpponents()
{
	_checkersHub.invoke('GetAvaiableOpponents');
}

function displayOnTable(avaiablePlayers)
{
	$('table#opponentAvaiable tr#player').remove();
	
	if(avaiablePlayers != undefined && avaiablePlayers.length > 0){
		for(var i = 0; i < avaiablePlayers.length; i++)
		{
			var tr = $("<tr id='player'></tr>");
			var tdName = $('<td>'+avaiablePlayers[i].Name+'</td>');
			tr.append(tdName);
			
			var tdButtonStart = $("<td><input type='button' value='Start match' onclick='startMatch()' </td>");
			tr.append(tdButtonStart);
			
			$('#opponentAvaiable').append(tr);
		}
	}
	else
	{
		var tr = $("<tr id='player'></tr>");
		var td = $('<td colspan=2 align=center>No players avaiable. </td>');
		tr.append(td);
		$('#opponentAvaiable').append(tr);
	}
}

function startMatch(connectionId)
{
	alert('not implemented yet!');
}

function registerPlayer()
{
	_checkersHub.invoke('RegisterPlayer', _name, _email);
}

function setBlackPieces()
{
	for(var y = 1; y <= 3; y++)
	{
		for(var x = 1; x <= 8; x++)
		{
			var color = "#000000";
			
			if((y%2 == 0))
			{
				if((x%2 == 0))
				{
					insertPiece(y,x,color);
				}
			}
			else
			{
				if((x%2 != 0))
				{
					insertPiece(y,x,color);
				}
			}
		}
	}
}

function setWhitePieces()
{
	for(var y = 8; y >= 6; y--)
	{
		for(var x = 1; x <= 8; x++)
		{
			var color = "#ffffff";
			
			if((y%2 == 0))
			{
				if((x%2 != 0))
				{
					insertPiece(y,x,color);
				}
			}
			else
			{
				if((x%2 == 0))
				{
					insertPiece(y,x,color);
				}
			}
		}
	}
}

function insertPiece(y, x, color)
{
	var piece = $("<div id='piece_"+y+x+"' style='background-color:"+color+"; border-radius:100%; width:40px; height:40px; border: 3px solid black;'></div>");
	$('#'+y+x).append(piece);
}

function drawTable()
{
	var table = $("<table border='1'> </table>");
	
	for(var y = 1; y <= 8; y++)
	{
		var row = $('<tr> </tr>');
		changeColorByRow(y);
		for(var x = 1; x <= 8; x++)
		{
			var column = $("<td width='60px' height='60px' bgcolor="+getColor()+" id="+ y+x +" align='center'> </td>");
			row.append(column);
		}
		table.append(row);
	}
	$('#table').append(table);
}

function changeColorByRow(row)
{
	if((row%2 == 0))
	{
		_color = "#839485";
	}
	else{
		_color = "#f2f2f2";
	}
}
function getColor()
{
	if(_color == "")
	{
		_color = "#f2f2f2";
	}
	
	if(_color == "#f2f2f2")
	{
		_color = "#839485";
	}
	else
	{
		_color = "#f2f2f2";
	}
	
	return _color;
}
