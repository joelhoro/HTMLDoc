angular.module('downloadlinks', [])

.factory('$blob', function () {

  function createURL(content,type) {
      var blob;
      blob = new Blob([content], {type: type});
      return (window.URL || window.webkitURL).createObjectURL(blob);    
  }

  return {
    csvToURL: function(content) {
      return createURL(content,'text/csv');
    },
    jsonToURL: function(obj) {
      var content = JSON.stringify(obj);
      return createURL(content,'application/json');
    },
    jsonTocsv: function(obj) {
        var csv = ""; 
        angular.forEach(obj,function(row,k) { 
            angular.forEach(row, function (col,j) { csv += col + ","; })
          csv += "\n";
        });
        return csv;
    },
    sanitizeFilename: function(name,ext) {
      var pat = new RegExp('^[A-Za-z0-9]+\.'+ext);
      if (pat.test(name)) {
        return name;
      }
      if (/^[A-Za-z0-9]+/.test(name)) {
        return name + "." + ext;
      }
      throw new Error("Invalid title for " + ext + " file : " + name);
    },
    revoke: function(url) {
      return (window.URL || window.webkitURL).revokeObjectURL(url);
    }
  };
})
 
.factory('$click', function() {
  return {
    on: function(element) {
      var e = document.createEvent("MouseEvent");
      e.initMouseEvent("click", false, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
      element.dispatchEvent(e);
    }
  };
})

.directive('downloadLink', function ($click, $blob, $log, $timeout) {
    // Usage: 
    //
    //  <button download-link tifle='SomeData' format='csv' data='{{data[0].data}}' headers='{{data[0].headers}}'>Download CSV</button>
    //

  return {
    compile: function($element, attr) {
      return function(scope, element, attr) {
         
        element.on('click', function(event) {
          var a_href, content, title, url, _ref;
          var data = JSON.parse(this.getAttribute('data'));
          var headers = JSON.parse(this.getAttribute('headers'));
          var title = this.getAttribute('title') || "Untitled";
          var format = this.getAttribute('format');

          if (!(content != null) && !(title != null)) {
            $log.warn("Invalid content or title in download-csv : ", content, title);
            return;
          }

          if (format == 'json') {
            title = $blob.sanitizeFilename(title,format);
            var obj = { data: data, headers: headers, title: title};
            url = $blob.jsonToURL(obj);          
          }
          else if (format == 'csv') {
            title = $blob.sanitizeFilename(title,format);
            var csv = $blob.jsonTocsv([headers].concat(data));
            url = $blob.csvToURL(csv);          
          }
          else {
            $log.warn("unknown format: ", format);
            return;
          }     

          element.append("<a download=\"" + title + "\" href=\"" + url + "\"></a>");
          a_href = element.find('a')[0];
           
          $click.on(a_href);
          $timeout(function() {$blob.revoke(url);});
           
          element[0].removeChild(a_href);
        });
      };
    }
  };
});