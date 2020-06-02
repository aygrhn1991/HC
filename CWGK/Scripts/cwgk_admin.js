var app = angular.module('app', []);
app.controller('LoginCtrl', function ($scope, $http) {
    $scope.login = function () {
        if (window.Util.isNull($scope.model.account) || window.Util.isNull($scope.model.password)) {
            layer.msg('账号或密码为空');
            return;
        }
        $http.post('/admin/login', $scope.model).success(function (data) {
            if (data.success) {
                window.Util.setCookie('admin', JSON.stringify(data.data));
                window.location.href = '/admin/index';
            } else {
                layer.msg(data.message);
            }
        });
    };
    $scope.model = {
        account: null,
        password: null
    };
});
app.controller('IndexCtrl', function ($scope) {
    $scope.admin = Common.getAdmin();
    $scope.logout = function () {
        window.Util.deleteCookie('admin');
        window.location.href = '/admin/login';
    };
});
app.controller('NewsTypeCtrl', function ($scope, $http) {
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/NewsType_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.add_show = function () {
        $scope.model = window.Util.getObject($scope.m);
        $('#modal').modal('toggle');
    };
    $scope.add = function () {
        if (window.Util.isNull($scope.model.name)) {
            layer.msg('分类名称为空');
            return;
        }
        $http.post('/admin/NewsType_Add', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.edit_show = function (e) {
        $scope.model = e;
        $('#modal').modal('toggle');
    };
    $scope.edit = function () {
        if (window.Util.isNull($scope.model.name)) {
            layer.msg('分类名称为空');
            return;
        }
        $http.post('/admin/NewsType_Edit', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.delete = function (e) {
        layer.confirm('确定删除？', {}, function () {
            $http.post('/admin/NewsType_Delete', { id: e.id }).success(function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $scope.get();
                }
            });
        });
    };
    $scope.makePage = function (data) {
        layui.laypage.render({
            elem: 'page',
            count: data.count,
            curr: $scope.search.page,
            limit: $scope.search.limit,
            limits: [10, 20, 30, 40, 50],
            layout: ['prev', 'page', 'next', 'count', 'limit'],
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.refresh = function () {
        window.location.reload();
    };
    $scope.m = {
        id: null,
        name: null
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.model = {};
        $scope.get();
    };
    $scope.reset();
});
app.controller('NewsCtrl', function ($scope, $http, $sce) {
    var ue = UE.getEditor('container');
    $scope.getXzqh = function () {
        $http.post('/common/GetXzqhByAdmin').success(function (data) {
            $scope.xzqh_search = [];
            $scope.xzqh_list = [];
            data.data.forEach(function (e) {
                if (e.pcode === '0') {
                    e.name = '-----' + e.name + '-----';
                }
                $scope.xzqh_search.push(e);
                $scope.xzqh_list.push(e);
            });
            $scope.xzqh_search.unshift({ code: '-1', name: '全部', pcode: '-1' });
        });
    };
    $scope.getXzqh();
    $scope.getType = function () {
        $http.post('/common/GetNewsType').success(function (data) {
            $scope.type_search = [];
            $scope.type_list = [];
            data.data.forEach(function (e) {
                $scope.type_search.push(e);
                $scope.type_list.push(e);
            });
            $scope.type_search.unshift({ id: -1, name: '全部' });
        });
    };
    $scope.getType();
    $scope.state_search = [
        { id: -1, name: '全部' },
        { id: 0, name: '审核中' },
        { id: 1, name: '已发布' }
    ];
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/News_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.add_show = function () {
        $scope.model = window.Util.getObject($scope.m);
        ue.setContent('');
        $('#modal').modal('toggle');
    };
    $scope.add = function () {
        if (window.Util.isNull($scope.model.xzqhcode)) {
            layer.msg('乡镇为空');
            return;
        }
        if (window.Util.isNull($scope.model.typeid)) {
            layer.msg('新闻分类为空');
            return;
        }
        if (window.Util.isNull($scope.model.title)) {
            layer.msg('新闻标题为空');
            return;
        }
        $scope.model.content = ue.getContent();
        $http.post('/admin/News_Add', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                //$scope.get();
                location.reload();
            }
        });
    };
    $scope.edit_show = function (e) {
        $scope.model = e;
        ue.setContent(e.content);
        $('#modal').modal('toggle');
    };
    $scope.edit = function () {
        if (window.Util.isNull($scope.model.title)) {
            layer.msg('新闻标题为空');
            return;
        }
        $scope.model.content = ue.getContent();
        $http.post('/admin/News_Edit', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                //$scope.get();
                location.reload();
            }
        });
    };
    $scope.delete = function (e) {
        layer.confirm('确定删除？', {}, function () {
            $http.post('/admin/News_Delete', { id: e.id }).success(function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $scope.get();
                }
            });
        });
    };
    $scope.preview = function (e) {
        $scope.model.html = $sce.trustAsHtml(e.content);
        $('#modal-preview').modal('toggle');
    };
    $scope.check = function (e, f) {
        layer.confirm('确定更改新闻发布状态？', {}, function () {
            $http.post('/admin/News_Check', { id: e.id, state: f }).success(function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $scope.get();
                }
            });
        });
    };
    $scope.makePage = function (data) {
        layui.laypage.render({
            elem: 'page',
            count: data.count,
            curr: $scope.search.page,
            limit: $scope.search.limit,
            limits: [10, 20, 30, 40, 50],
            layout: ['prev', 'page', 'next', 'count', 'limit'],
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.refresh = function () {
        window.location.reload();
    };
    $scope.m = {
        id: null,
        xzqhcode: null,
        typeid: null,
        title: null,
        time: null,
        content: null,
        state: null,
        /////
        html: null
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.search.string1 = '-1';
        $scope.search.number1 = $scope.search.number2 = -1;
        $scope.model = {};
        $scope.get();
    };
    $scope.reset();
});
app.controller('NewsTalkCtrl', function ($scope, $http) {
    $scope.getXzqh = function () {
        $http.post('/common/GetXzqhByAdmin').success(function (data) {
            $scope.xzqh_search = [];
            $scope.xzqh_list = [];
            data.data.forEach(function (e) {
                if (e.pcode === '0') {
                    e.name = '-----' + e.name + '-----';
                }
                $scope.xzqh_search.push(e);
                $scope.xzqh_list.push(e);
            });
            $scope.xzqh_search.unshift({ code: '-1', name: '全部', pcode: '-1' });
        });
    };
    $scope.getXzqh();
    $scope.getType = function () {
        $http.post('/common/GetNewsType').success(function (data) {
            $scope.type_search = [];
            $scope.type_list = [];
            data.data.forEach(function (e) {
                $scope.type_search.push(e);
                $scope.type_list.push(e);
            });
            $scope.type_search.unshift({ id: -1, name: '全部' });
        });
    };
    $scope.getType();
    $scope.state_search = [
        { id: -1, name: '全部' },
        { id: 0, name: '未回复' },
        { id: 1, name: '已回复' }
    ];
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/NewsTalk_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.edit_show = function (e) {
        $scope.model = e;
        $scope.getFile();
        $('#modal').modal('toggle');
    };
    $scope.edit = function () {
        if (window.Util.isNull($scope.model.answer)) {
            layer.msg('回复内容为空');
            return;
        }
        $http.post('/admin/NewsTalk_Edit', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.delete = function (e) {
        layer.confirm('确定删除？', {}, function () {
            $http.post('/admin/NewsTalk_Delete', { id: e.id }).success(function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $scope.get();
                }
            });
        });
    };
    $scope.getFile = function () {
        $http.post('/admin/NewsTalk_GetFile', { id: $scope.model.id }).success(function (data) {
            if (data.success) {
                $scope.files = data.data;
            }
        });
    };
    $scope.makePage = function (data) {
        layui.laypage.render({
            elem: 'page',
            count: data.count,
            curr: $scope.search.page,
            limit: $scope.search.limit,
            limits: [10, 20, 30, 40, 50],
            layout: ['prev', 'page', 'next', 'count', 'limit'],
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.refresh = function () {
        window.location.reload();
    };
    $scope.m = {
        id: null,
        newsid: null,
        userid: null,
        adminid: null,
        time: null,
        question: null,
        answer: null
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.search.string1 = '-1';
        $scope.search.number1 = $scope.search.number2 = -1;
        $scope.model = {};
        $scope.get();
    };
    $scope.reset();
});
app.controller('AdminCtrl', function ($scope, $http) {
    $scope.getXzqh = function () {
        $http.post('/common/GetXzqhByAdmin').success(function (data) {
            $scope.xzqh_search = [];
            $scope.xzqh_list = [];
            data.data.forEach(function (e) {
                if (e.pcode === '0') {
                    e.name = '-----' + e.name + '-----';
                }
                $scope.xzqh_search.push(e);
                $scope.xzqh_list.push(e);
            });
            $scope.xzqh_search.unshift({ code: '-1', name: '全部', pcode: '-1' });
        });
    };
    $scope.getXzqh();
    $scope.$watch('model.xzqhcode', function (newValue, oldValue) {
        if (window.Util.isNull(newValue)) {
            return;
        }
        if (newValue.length === 9) {
            $scope.model.level = 2;
        }
        if (newValue.length === 15) {
            $scope.model.level = 3;
        }
    });
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/Admin_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.add_show = function () {
        $scope.model = window.Util.getObject($scope.m);
        $('#modal').modal('toggle');
    };
    $scope.add = function () {
        if (window.Util.isNull($scope.model.xzqhcode)) {
            layer.msg('乡镇为空');
            return;
        }
        if (window.Util.isNull($scope.model.account)) {
            layer.msg('账号为空');
            return;
        }
        if (window.Util.isNull($scope.model.password)) {
            layer.msg('密码为空');
            return;
        }
        if ($scope.model.password !== $scope.model.confirmpassword) {
            layer.msg('两次密码不一致');
            return;
        }
        if (window.Util.isNull($scope.model.name)) {
            layer.msg('姓名为空');
            return;
        }
        if (window.Util.isNull($scope.model.phone)) {
            layer.msg('电话为空');
            return;
        }
        $http.post('/admin/Admin_Add', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.edit_show = function (e) {
        $scope.model = e;
        $('#modal').modal('toggle');
    };
    $scope.edit = function () {
        if (window.Util.isNull($scope.model.name)) {
            layer.msg('姓名为空');
            return;
        }
        if (window.Util.isNull($scope.model.phone)) {
            layer.msg('电话为空');
            return;
        }
        $http.post('/admin/Admin_Edit', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.delete = function (e) {
        layer.confirm('确定删除？', {}, function () {
            $http.post('/admin/Admin_Delete', { id: e.id }).success(function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $scope.get();
                }
            });
        });
    };
    $scope.password = function (e) {
        $scope.defaultPassword = '123456';
        layer.confirm('确定重置密码为' + $scope.defaultPassword + '？', {}, function () {
            $http.post('/admin/Admin_Password', { id: e.id, password: $scope.defaultPassword }).success(function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $scope.get();
                }
            });
        });
    };
    $scope.makePage = function (data) {
        layui.laypage.render({
            elem: 'page',
            count: data.count,
            curr: $scope.search.page,
            limit: $scope.search.limit,
            limits: [10, 20, 30, 40, 50],
            layout: ['prev', 'page', 'next', 'count', 'limit'],
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.refresh = function () {
        window.location.reload();
    };
    $scope.m = {
        id: null,
        level: null,
        xzqhcode: null,
        account: null,
        password: null,
        name: null,
        phone: null,
        /////
        confirmpassword: null
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.search.string1 = '-1';
        $scope.model = {};
        $scope.get();
    };
    $scope.reset();
});
app.controller('UserCtrl', function ($scope, $http) {
    $scope.getXzqh = function () {
        $http.post('/common/GetXzqh').success(function (data) {
            $scope.xzqh = data.data;
        });
    };
    $scope.getXzqh();
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/User_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.edit_xzqh_show = function (e) {
        $scope.model = e;
        $scope.xzqh.forEach(function (e) {
            e.select = false;
        });
        $http.post('/admin/User_GetXzqh', { id: e.id }).success(function (data) {
            if (data.data.length !== 0) {
                $scope.xzqh.forEach(function (f) {
                    if (data.data.filter(function (g) { return g.code === f.code; }).length > 0) {
                        f.select = true;
                    } else {
                        f.select = false;
                    }
                });
            }
        });
        $('#modal-xzqh').modal('toggle');
    };
    $scope.edit_xzqh = function () {
        var list = [];
        $scope.xzqh.forEach(function (e) {
            if (e.select) {
                list.push(e.code);
            }
        });
        if (list.length === 0) {
            layer.msg('至少选择一个关注乡镇');
            return;
        }
        $http.post('/admin/User_AddXzqh', { id: $scope.model.id, list: list }).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal-xzqh').modal('toggle');
            }
        });
    };
    $scope.check = function (e) {
        e.select = !e.select;
    };
    $scope.makePage = function (data) {
        layui.laypage.render({
            elem: 'page',
            count: data.count,
            curr: $scope.search.page,
            limit: $scope.search.limit,
            limits: [10, 20, 30, 40, 50],
            layout: ['prev', 'page', 'next', 'count', 'limit'],
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.refresh = function () {
        window.location.reload();
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.model = {};
        $scope.get();
    };
    $scope.reset();
});
app.controller('UserRankCtrl', function ($scope, $http) {
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/UserRank_GetList').success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.scan = data.data.scan;
                $scope.question = data.data.question;
                $scope.duration = data.data.duration;
                $scope.duration.forEach(function (e) {
                    e.count_duration = window.Util.secondToHHMMSS(e.count_duration);
                });
            }
        });
    };
    $scope.refresh = function () {
        window.location.reload();
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.get();
    };
    $scope.reset();

});
app.controller('PasswordCtrl', function ($scope, $http) {
    $scope.edit = function () {
        if (window.Util.isNull($scope.model.oldPassword) || window.Util.isNull($scope.model.newPassword) || window.Util.isNull($scope.model.confirmPassword)) {
            layer.msg('密码为空');
            return;
        }
        if ($scope.model.newPassword !== $scope.model.confirmPassword) {
            layer.msg('两次密码不一致');
            return;
        }
        $scope.loading = layer.load();
        $http.post('/admin/Password_Edit', $scope.model).success(function (data) {
            layer.close($scope.loading);
            layer.msg(data.message);
            if (data.success) {
                $scope.model = window.Util.getObject($scope.m);
            }
        });
    };
    $scope.refresh = function () {
        window.location.reload();
    };
    $scope.m = {
        oldPassword: null,
        newPassword: null,
        confirmPassword: null
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.model = window.Util.getObject($scope.m);
    };
    $scope.reset();
});