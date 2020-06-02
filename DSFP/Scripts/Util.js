window.Util = {
    getObject: function (obj) {
        return JSON.parse(JSON.stringify(obj));
    },
    getSearchObject: function () {
        var obj = {
            page: 1,
            limit: 10,
            string1: null,
            string2: null,
            number1: null,
            number2: null,
            datetime1: null,
            datetime2: null
        };
        return JSON.parse(JSON.stringify(obj));
    },
    getQueryVariable(variable) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] === variable) { return pair[1]; }
        }
        return false;
    },
    isNull: function (obj) {
        return (obj === undefined || obj === null || obj === '') ? true : false;
    },
    setCookie: function (name, value) {
        document.cookie = name + '=' + escape(value) + ';path=/';
    },
    getCookie: function (name) {
        var reg = new RegExp('(^| )' + name + '=([^;]*)(;|$)');
        var arr = document.cookie.match(reg);
        if (arr) {
            return unescape(arr[2]);
        } else {
            return null;
        }
    },
    deleteCookie: function (name) {
        var date = new Date();
        date.setTime(date.getTime() - 10000);
        document.cookie = name + "=v; expires=" + date.toGMTString() + ';path=/';
    },
    startWith: function (origin, str) {
        var reg = new RegExp("^" + str);
        return reg.test(origin);
    },
    dateToYYMMDD: function (date) {
        var y = date.getFullYear();
        var M = ((date.getMonth() + 1) >= 10 ? '' : '0') + (date.getMonth() + 1);
        var d = (date.getDate() >= 10 ? '' : '0') + date.getDate();
        return y + '-' + M + '-' + d;
    },
    dateToYYMMDDHHMMSS: function (date) {
        var y = date.getFullYear();
        var M = ((date.getMonth() + 1) >= 10 ? '' : '0') + (date.getMonth() + 1);
        var d = (date.getDate() >= 10 ? '' : '0') + date.getDate();
        var h = (date.getHours() >= 10 ? '' : '0') + date.getHours();
        var m = (date.getMinutes() >= 10 ? '' : '0') + date.getMinutes();
        var s = (date.getSeconds() >= 10 ? '' : '0') + date.getSeconds();
        return y + '-' + M + '-' + d + ' ' + h + ':' + m + ':' + s;
    },
    _dateToYYMMDDHHMMSS: function (date) {
        var y = date.getFullYear();
        var M = ((date.getMonth() + 1) >= 10 ? '' : '0') + (date.getMonth() + 1);
        var d = (date.getDate() >= 10 ? '' : '0') + date.getDate();
        var h = (date.getHours() >= 10 ? '' : '0') + date.getHours();
        var m = (date.getMinutes() >= 10 ? '' : '0') + date.getMinutes();
        var s = (date.getSeconds() >= 10 ? '' : '0') + date.getSeconds();
        return y + M + d + h + m + s;
    },
    secondToHHMMSS: function (seconds) {
        var temp = 0;
        var str = '';
        temp = parseInt(seconds / 3600);
        str += (temp < 10) ? '0' + temp + ':' : '' + temp + ':';
        temp = parseInt(seconds % 3600 / 60);
        str += (temp < 10) ? '0' + temp + ':' : '' + temp + ':';
        temp = parseInt(seconds % 3600 % 60);
        str += (temp < 10) ? '0' + temp : '' + temp;
        return str;
    },
    addYear: function (date, year) {
        var y = date.getFullYear();
        return new Date(date.setFullYear(y + year));
    }
};