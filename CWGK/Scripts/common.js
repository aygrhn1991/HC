window.Common = {
    getAdmin: function () {
        var cookie = window.Util.getCookie('admin');
        return JSON.parse(cookie);
    },
    getUser: function () {
        var cookie = window.Util.getCookie('user');
        return JSON.parse(unescape(cookie));
    }
};