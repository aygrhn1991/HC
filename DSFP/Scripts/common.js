Common = {
    getAdmin: function () {
        var cookie = Util.getCookie('admin');
        return JSON.parse(cookie);
    },
    getUser: function () {
        var cookie = Util.getCookie('user');
        return JSON.parse(cookie);
    }
};