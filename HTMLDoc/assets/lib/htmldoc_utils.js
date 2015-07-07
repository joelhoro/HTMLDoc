// Extensions to base types

Array.prototype.pickrandom = function() {
	return this[Math.floor( Math.random()*this.length)];
}


Date.prototype.AddDays = function(n) { 
	return new Date(this.getTime()+n*1000*24*3600); 
}

// Turn data into a html table
function DataToTable(headers,data,tableId) {
	console.log('Filling table ', tableId, 'with', headers.length, 'columns (',headers,') on', data.length, 'rows: ', data);
    var output = "";
    var headersHtml = "<tr>";
    $.each(headers, function(idx) { headersHtml += "<th>"+this+"</th>" });
    headersHtml += "</tr>";
    $(tableId + " thead").html(headersHtml);

	var table = ""; 
	$.each(data, function(idx) { 
		table += "\t<tr>\n"; 
		$.each(this,function(col) { table += "\t\t<td>" + this + "</td>\n" } ); 
		table += "\t</tr>\n"; 
	} ); 
	$(tableId + " tbody").html(table);
}

function DataInfoToTable(info) {
	DataToTable(info.headers,info.data,info.link);
}


var BlobToLink = function(blob,fileName) {
	var url  = URL.createObjectURL(blob);
	var link = document.createElement('a');
	link.download    = fileName;
	link.href        = url;
	return link
}

function CsvLink(alldata,fileName) {
	var csv = "";
	$.each(alldata,function(idx) { 
		$.each(this,function(col) { csv += this + ","; })
		csv += "\n";
	});
	var blob = new Blob([csv], {type: "application/csv"});
	return BlobToLink(blob,fileName);
}

function JsonLink(alldata,fileName) {
	var json = JSON.stringify(alldata);
	var blob = new Blob([json], {type: "application/json"});
	return BlobToLink(blob,fileName);
}
