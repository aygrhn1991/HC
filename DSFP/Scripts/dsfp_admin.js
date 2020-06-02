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
app.controller('TeamAbilityCtrl', function ($scope, $http) {
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/TeamAbility_GetListByPage', $scope.search).success(function (data) {
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
            layer.msg('服务名称为空');
            return;
        }
        $http.post('/admin/TeamAbility_Add', $scope.model).success(function (data) {
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
            layer.msg('服务名称为空');
            return;
        }
        $http.post('/admin/TeamAbility_Edit', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.delete = function (e) {
        layer.confirm('确定删除？', {}, function () {
            $http.post('/admin/TeamAbility_Delete', { id: e.id }).success(function (data) {
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
        $scope.get();
    };
    $scope.reset();
});
app.controller('TeamCtrl', function ($scope, $http) {
    $scope.getAbility = function () {
        $http.post('/common/GetTeamAbility').success(function (data) {
            $scope.ability = data.data;
        });
    };
    $scope.getAbility();
    $scope.getUser = function () {
        $http.post('/common/GetUser').success(function (data) {
            $scope.user = data.data;
        });
    };
    $scope.getUser();
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/Team_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.add_show = function () {
        $scope.model = window.Util.getObject($scope.m);
        setTimeout(function () {
            $scope.transfer = layui.transfer.render({
                id: 'transfer',
                elem: '#transfer',
                parseData: function (res) {
                    return {
                        value: res.id,
                        title: res.name,
                        disabled: res.disabled,
                        checked: res.checked
                    };
                },
                title: ['未选', '已选'],
                data: $scope.user,
                showSearch: true
            });
        }, 500);
        $('#modal').modal('toggle');
    };
    $scope.add = function () {
        if (window.Util.isNull($scope.model.name)) {
            layer.msg('姓名为空');
            return;
        }
        if (window.Util.isNull($scope.model.abilityid)) {
            layer.msg('服务为空');
            return;
        }
        var ids = [];
        $scope.transfer.getData('transfer').forEach(function (e) {
            ids.push(e.value);
        });
        if (ids.length === 0) {
            layer.msg('请至少选择一名网格员');
            return;
        }
        $scope.model.leaderid = ids[0];
        $http.post('/admin/Team_Add', { model: $scope.model, ids: ids }).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.edit_show = function (e) {
        $scope.model = e;
        $http.post('/admin/Team_GetTeamUser', { id: $scope.model.id }).success(function (data) {
            var selectUser = [];
            data.data.forEach(function (e) {
                selectUser.push(e.id);
            });
            setTimeout(function () {
                $scope.transfer = layui.transfer.render({
                    id: 'transfer',
                    elem: '#transfer',
                    parseData: function (res) {
                        return {
                            value: res.id,
                            title: res.name,
                            disabled: res.disabled,
                            checked: res.checked
                        };
                    },
                    title: ['未选', '已选'],
                    data: $scope.user,
                    value: selectUser,
                    showSearch: true
                });
            }, 500);
        });
        $('#modal').modal('toggle');
    };
    $scope.edit = function () {
        if (window.Util.isNull($scope.model.name)) {
            layer.msg('姓名为空');
            return;
        }
        if (window.Util.isNull($scope.model.abilityid)) {
            layer.msg('服务为空');
            return;
        }
        var ids = [];
        $scope.transfer.getData('transfer').forEach(function (e) {
            ids.push(e.value);
        });
        if (ids.length === 0) {
            layer.msg('请至少选择一名网格员');
            return;
        }
        $scope.model.leaderid = ids[0];
        $http.post('/admin/Team_Edit', { model: $scope.model, ids: ids }).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.delete = function (e) {
        layer.confirm('确定删除？', {}, function () {
            $http.post('/admin/Team_Delete', { id: e.id }).success(function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $scope.get();
                }
            });
        });
    };
    $scope.preview = function (e) {
        $scope.model = e;
        $http.post('/admin/Team_GetTeamUser', { id: $scope.model.id }).success(function (data) {
            $scope.member = data.data;
        });
        $('#modal-preview').modal('toggle');
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
        name: null,
        abilityid: null,
        leaderid: null
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.get();
    };
    $scope.reset();
});
app.controller('UserCtrl', function ($scope, $http) {
    $scope.getAbility = function () {
        $http.post('/common/GetTeamAbility').success(function (data) {
            $scope.ability = data.data;
        });
    };
    $scope.getAbility();
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
    $scope.edit_show = function (e) {
        $scope.model = e;
        $('#modal').modal('toggle');
    };
    $scope.edit = function () {
        $http.post('/admin/User_Edit', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.preview = function (e) {
        $scope.model = e;
        $http.post('/admin/User_GetUserTeam', { id: $scope.model.id }).success(function (data) {
            $scope.team = data.data;
        });
        $('#modal-preview').modal('toggle');
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
        $scope.get();
    };
    $scope.reset();
});
app.controller('XzqhCtrl', function ($scope, $http) {
    $scope.getXzqh = function () {
        $http.post('/common/GetXzqh').success(function (data) {
            $scope.xzqh_search = [];
            data.data.forEach(function (e) {
                if (e.pcode === '0') {
                    $scope.xzqh_search.push(e);
                }
            });
            $scope.xzqh_search.unshift({ code: '-1', name: '全部', pcode: '-1' });
        });
    };
    $scope.getXzqh();
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/Xzqh_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.preview = function (e) {
        $scope.model = e;
        $http.post('/admin/Xzqh_GetXzqhPoor', { code: $scope.model.code }).success(function (data) {
            $scope.poor = data.data;
        });
        $('#modal-preview').modal('toggle');
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
        $scope.search.string1 = '-1';
        $scope.get();
    };
    $scope.reset();
});
app.controller('PoorCtrl', function ($scope, $http) {
    $scope.getXzqh = function () {
        $http.post('/common/GetXzqh').success(function (data) {
            var copy1 = JSON.parse(JSON.stringify(data.data));
            var copy2 = JSON.parse(JSON.stringify(data.data));
            $scope.xzqh_search = [];
            $scope.xzqh_list = [];
            var zhen = '';
            copy1.forEach(function (e) {
                if (e.pcode === '0') {
                    e.name = '-----' + e.name + '-----';
                }
                $scope.xzqh_search.push(e);
            });
            copy2.forEach(function (e) {
                if (e.pcode === '0') {
                    zhen = e.name;
                } else {
                    e.name = zhen + '-----' + e.name;
                    $scope.xzqh_list.push(e);
                }
            });
            $scope.xzqh_search.unshift({ code: '-1', name: '全部', pcode: '-1' });
        });
    };
    $scope.getXzqh();
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/Poor_GetListByPage', $scope.search).success(function (data) {
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
            layer.msg('姓名为空');
            return;
        }
        if (window.Util.isNull($scope.model.xzqhcode)) {
            layer.msg('乡镇为空');
            return;
        }
        if (window.Util.isNull($scope.model.population)) {
            layer.msg('家庭人口为空');
            return;
        }
        $http.post('/admin/Poor_Add', $scope.model).success(function (data) {
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
        if (window.Util.isNull($scope.model.xzqhcode)) {
            layer.msg('乡镇为空');
            return;
        }
        if (window.Util.isNull($scope.model.population)) {
            layer.msg('家庭人口为空');
            return;
        }
        $http.post('/admin/Poor_Edit', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.delete = function (e) {
        layer.confirm('确定删除？', {}, function () {
            $http.post('/admin/Poor_Delete', { id: e.id }).success(function (data) {
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
        name: null,
        xzqhcode: null,
        population: null
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.search.string1 = '-1';
        $scope.get();
    };
    $scope.reset();
});
app.controller('SignCtrl', function ($scope, $http) {
    $scope.getPoor = function () {
        $http.post('/common/GetPoor', { name: $scope.search.string2 }).success(function (data) {
            $scope.poor = data.data;
        });
    };
    $scope.year = [];
    for (var i = 0; i < 10; i++) {
        $scope.year.push(2020 + i);
    }
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/Sign_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                data.data.forEach(function (e) {
                    e.time = window.Util.dateToYYMMDD(new Date(e.time));
                    e.datestart = window.Util.dateToYYMMDD(new Date(e.datestart));
                    e.dateend = window.Util.dateToYYMMDD(new Date(e.dateend));
                });
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.add_show = function () {
        $scope.currentStep = 1;
        $scope.model = window.Util.getObject($scope.m);
        $scope.model.number = window.Util._dateToYYMMDDHHMMSS(new Date());
        $scope.model.time = window.Util.dateToYYMMDD(new Date());
        $scope.model.datestart = window.Util.dateToYYMMDD(new Date());
        $scope.model.dateend = window.Util.dateToYYMMDD(window.Util.addYear(new Date(), 1));
        layui.laydate.render({
            elem: '#time1',
            value: $scope.model.time,
            done: function (value, date) {
                $scope.model.time = value;
            }
        });
        layui.laydate.render({
            elem: '#time2',
            range: true,
            value: $scope.model.datestart + ' - ' + $scope.model.dateend,
            done: function (value, date) {
                $scope.model.datestart = value.split(' - ')[0];
                $scope.model.dateend = value.split(' - ')[1];
            }
        });
        $('#modal').modal('toggle');
    };
    $scope.add = function () {
        $http.post('/admin/Sign_Add', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.edit_show = function (e) {
        $scope.currentStep = 1;
        $scope.model = e;
        layui.laydate.render({
            elem: '#time1',
            value: $scope.model.time,
            done: function (value, date) {
                $scope.model.time = value;
            }
        });
        layui.laydate.render({
            elem: '#time2',
            range: true,
            value: $scope.model.datestart + ' - ' + $scope.model.dateend,
            done: function (value, date) {
                $scope.model.datestart = value.split(' - ')[0];
                $scope.model.dateend = value.split(' - ')[1];
            }
        });
        $('#modal').modal('toggle');
    };
    $scope.edit = function () {
        $http.post('/admin/Sign_Edit', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.delete = function (e) {
        layer.confirm('确定删除？', {}, function () {
            $http.post('/admin/Sign_Delete', { id: e.id }).success(function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $scope.get();
                }
            });
        });
    };
    $scope.selectPoor = function (e) {
        $scope.model.poorid = e.id;
        $scope.model.poorname = e.name;
    };
    $scope.step = function (e) {
        if (e > 0) {
            if ($scope.currentStep === 1) {
                if (window.Util.isNull($scope.model.poorid)) {
                    layer.msg('请选择一名扶贫对象');
                    return;
                }
            }
            if ($scope.currentStep === 2) {
                if (window.Util.isNull($scope.model.name)) {
                    layer.msg('请填写签约名称');
                    return;
                }
                if (window.Util.isNull($scope.model.weight)) {
                    layer.msg('请填写水稻产量');
                    return;
                }
                if (window.Util.isNull($scope.model.price)) {
                    layer.msg('请填写签约单价');
                    return;
                }
            }
        }
        $scope.currentStep += e;
    };
    $scope.preview = function (e) {
        $scope.model = e;
        $('#modal-preview').modal('toggle');
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
        poorid: null,
        name: null,
        number: null,
        time: null,
        datestart: null,
        dateend: null,
        weight: null,
        price: null,
        /////
        poorname: null
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.search.number1 = new Date().getFullYear();
        $scope.currentStep = 1;
        $scope.getPoor();
        $scope.get();
    };
    $scope.reset();
});
app.controller('ActivityCtrl', function ($scope, $http) {
    $scope.getAbility = function () {
        $http.post('/common/GetTeamAbility').success(function (data) {
            $scope.ability = data.data;
        });
    };
    $scope.getAbility();
    $scope.getTeam = function () {
        $http.post('/common/GetTeam').success(function (data) {
            $scope.team = data.data;
        });
    };
    $scope.getTeam();
    $scope.getPoor = function () {
        $http.post('/common/GetPoor').success(function (data) {
            $scope.poor = [];
            data.data.forEach(function (e) {
                e.select = false;
                $scope.poor.push(e);
            });
        });
    };
    $scope.getPoor();
    $scope.year = [];
    for (var i = 0; i < 10; i++) {
        $scope.year.push(2020 + i);
    }
    $scope.state_search = [
        { id: -1, name: '全部' },
        { id: 0, name: '未完成' },
        { id: 1, name: '已完成' }
    ];
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/admin/Activity_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                data.data.forEach(function (e) {
                    e.time = window.Util.dateToYYMMDD(new Date(e.time));
                });
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.add_show = function () {
        $scope.model = window.Util.getObject($scope.m);
        $scope.model.time = window.Util.dateToYYMMDD(new Date());
        layui.laydate.render({
            elem: '#time1',
            value: $scope.model.time,
            done: function (value, date) {
                $scope.model.time = value;
            }
        });
        setTimeout(() => {
            $scope.transfer = layui.transfer.render({
                id: 'transfer',
                elem: '#transfer',
                parseData: function (res) {
                    return {
                        value: res.id,
                        title: res.name,
                        disabled: res.disabled,
                        checked: res.checked
                    };
                },
                title: ['未选', '已选'],
                data: $scope.poor,
                showSearch: true
            });
        }, 500);
        $('#modal').modal('toggle');
    };
    $scope.add = function () {
        if (window.Util.isNull($scope.model.name)) {
            layer.msg('姓名为空');
            return;
        }
        if (window.Util.isNull($scope.model.teamid)) {
            layer.msg('小组为空');
            return;
        }
        if (window.Util.isNull($scope.model.time)) {
            layer.msg('时间为空');
            return;
        }
        var ids = [];
        $scope.transfer.getData('transfer').forEach(function (e) {
            ids.push(e.value);
        });
        if (ids.length === 0) {
            layer.msg('请至少选择一名扶贫对象');
            return;
        }
        $http.post('/admin/Activity_Add', { model: $scope.model, ids: ids }).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.edit_show = function (e) {
        $scope.model = e;
        layui.laydate.render({
            elem: '#time1',
            value: $scope.model.time,
            done: function (value, date) {
                $scope.model.time = value;
            }
        });
        $http.post('/admin/Activity_GetActivityPoor', { id: $scope.model.id }).success(function (data) {
            var selectPoor = [];
            data.data.forEach(function (e) {
                selectPoor.push(e.id);
            });
            setTimeout(function () {
                $scope.transfer = layui.transfer.render({
                    id: 'transfer',
                    elem: '#transfer',
                    parseData: function (res) {
                        return {
                            value: res.id,
                            title: res.name,
                            disabled: res.disabled,
                            checked: res.checked
                        };
                    },
                    title: ['未选', '已选'],
                    data: $scope.poor,
                    value: selectPoor,
                    showSearch: true
                });
            }, 500);
        });
        $('#modal').modal('toggle');
    };
    $scope.edit = function () {
        if (window.Util.isNull($scope.model.name)) {
            layer.msg('姓名为空');
            return;
        }
        if (window.Util.isNull($scope.model.teamid)) {
            layer.msg('小组为空');
            return;
        }
        if (window.Util.isNull($scope.model.time)) {
            layer.msg('时间为空');
            return;
        }
        var ids = [];
        $scope.transfer.getData('transfer').forEach(function (e) {
            ids.push(e.value);
        });
        if (ids.length === 0) {
            layer.msg('请至少选择一名扶贫对象');
            return;
        }
        $http.post('/admin/Activity_Edit', { model: $scope.model, ids: ids }).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal').modal('toggle');
                $scope.get();
            }
        });
    };
    $scope.delete = function (e) {
        layer.confirm('确定删除？', {}, function () {
            $http.post('/admin/Activity_Delete', { id: e.id }).success(function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $scope.get();
                }
            });
        });
    };
    $scope.getActivityPoor = function () {
        $http.post('/admin/Activity_GetActivityPoor', { id: $scope.model.id }).success(function (data) {
            data.data.forEach(function (e) {
                e.able = false;
            });
            $scope.selectPoor = data.data;
        });
    };
    $scope.preview = function (e) {
        $scope.model = e;
        $scope.getActivityPoor();
        $('#modal-preview').modal('toggle');
    };
    $scope.setProfit = function (e) {
        $http.post('/admin/Activity_EditPoorProfit', { activityid: $scope.model.id, poorid: e.id, profit: e.profit }).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $scope.getActivityPoor();
            }
        });
    };
    $scope.finish_show = function (e) {
        $scope.model = e;
        $http.post('/admin/Activity_GetActivityPoor', { id: $scope.model.id }).success(function (data) {
            var finishCount = 0;
            data.data.forEach(function (e) {
                if (!window.Util.isNull(e.profit) && e.profit !== 0) {
                    finishCount++;
                }
            });
            if (finishCount === data.data.length) {
                $('#modal-finish').modal('toggle');
            } else {
                layer.msg('有扶贫对象尚未完成收益填报，活动无法设置完成');
            }
        });
    };
    $scope.finish = function () {
        if (window.Util.isNull($scope.model.result)) {
            layer.msg('成果反馈为空');
            return;
        }
        if (window.Util.isNull($scope.model.suggest)) {
            layer.msg('意见建议为空');
            return;
        }
        $http.post('/admin/Activity_Check', $scope.model).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $('#modal-finish').modal('toggle');
                $scope.get();
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
        name: null,
        number: null,
        teamid: null,
        abilityid: null,
        time: null,
        result: null,
        suggest: null
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.search.number1 = new Date().getFullYear();
        $scope.search.number2 = -1;
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