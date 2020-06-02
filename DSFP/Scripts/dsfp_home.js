var app = angular.module('app', ['ngRoute']);
app.config(function ($routeProvider) {
    $routeProvider
        .when('/sign', {
            templateUrl: '/home/sign',
            controller: 'SignCtrl'
        })
        .when('/activity', {
            templateUrl: '/home/activity',
            controller: 'ActivityCtrl'
        })
        .when('/my', {
            templateUrl: '/home/my',
            controller: 'MyCtrl'
        })
        .when('/myteam', {
            templateUrl: '/home/myteam',
            controller: 'MyTeamCtrl'
        })
        .when('/myxzqh', {
            templateUrl: '/home/myxzqh',
            controller: 'MyXzqhCtrl'
        })
        .when('/mypoor', {
            templateUrl: '/home/mypoor',
            controller: 'MyPoorCtrl'
        })
        .otherwise({
            redirectTo: '/activity'
        });
});
app.controller('IndexCtrl', function ($rootScope) {
    $rootScope.back = function () {
        window.history.back();
    };
});
app.controller('SignCtrl', function ($scope, $http) {
    $scope.year = [];
    for (var i = 0; i < 10; i++) {
        $scope.year.push(2020 + i);
    }
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/home/Sign_GetListByPage', $scope.search).success(function (data) {
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
    $scope.makePage = function (data) {
        layui.laypage.render({
            elem: 'page',
            count: data.count,
            curr: $scope.search.page,
            limit: $scope.search.limit,
            limits: [10, 20, 30, 40, 50],
            layout: ['prev', 'page', 'next', 'count'],
            groups: 1,
            prev: '<<',
            next: '>>',
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.search.number1 = new Date().getFullYear();
        $scope.get();
    };
    $scope.reset();
});
app.controller('ActivityCtrl', function ($scope, $http) {
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
        $http.post('/home/Activity_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                data.data.forEach(function (e) {
                    e.time = window.Util.dateToYYMMDD(new Date(e.time));
                });
                $scope.data = data.data;
                if (data.data.length !== 0) {
                    $scope.preview($scope.data[0]);
                }
                $scope.makePage(data);
            }
        });
    };
    $scope.preview = function (e) {
        $http.post('/home/Activity_GetActivityPoor', { id: e.id }).success(function (data) {
            e.poor = data.data;
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
            layout: ['prev', 'page', 'next', 'count'],
            groups: 1,
            prev: '<<',
            next: '>>',
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
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
app.controller('MyCtrl', function ($scope, $http) {
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/home/My_Get').success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data[0];
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.get();
    };
    $scope.reset();
});
app.controller('MyTeamCtrl', function ($scope, $http) {
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/home/MyTeam_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                if (data.data.length !== 0) {
                    $scope.preview($scope.data[0]);
                }
                $scope.makePage(data);
            }
        });
    };
    $scope.preview = function (e) {
        $http.post('/home/MyTeam_GetTeamUser', { id: e.id }).success(function (data) {
            e.member = data.data;
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
            layout: ['prev', 'page', 'next', 'count'],
            groups: 1,
            prev: '<<',
            next: '>>',
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.get();
    };
    $scope.reset();
});
app.controller('MyXzqhCtrl', function ($scope, $http) {
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
        $http.post('/home/MyXzqh_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                if (data.data.length !== 0) {
                    $scope.preview($scope.data[0]);
                }
                $scope.makePage(data);
            }
        });
    };
    $scope.preview = function (e) {
        $http.post('/home/MyXzqh_GetXzqhPoor', { code: e.code }).success(function (data) {
            e.poor = data.data;
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
            layout: ['prev', 'page', 'next', 'count'],
            groups: 1,
            prev: '<<',
            next: '>>',
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.search.string1 = '-1';
        $scope.get();
    };
    $scope.reset();
});
app.controller('MyPoorCtrl', function ($scope, $http) {
    $scope.getXzqh = function () {
        $http.post('/common/GetXzqh').success(function (data) {
            var copy1 = JSON.parse(JSON.stringify(data.data));
            $scope.xzqh_search = [];
            var zhen = '';
            copy1.forEach(function (e) {
                if (e.pcode === '0') {
                    e.name = '-----' + e.name + '-----';
                }
                $scope.xzqh_search.push(e);
            });
            $scope.xzqh_search.unshift({ code: '-1', name: '全部', pcode: '-1' });
        });
    };
    $scope.getXzqh();
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/home/MyPoor_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                $scope.makePage(data);
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
            layout: ['prev', 'page', 'next', 'count'],
            groups: 1,
            prev: '<<',
            next: '>>',
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = window.Util.getSearchObject();
        $scope.search.string1 = '-1';
        $scope.get();
    };
    $scope.reset();
});






app.controller('NewsTypeCtrl', function ($scope, $http) {
    $scope.getNewsType = function () {
        $http.post('/common/GetNewsType').success(function (data) {
            if (data.data.length !== 0) {
                $scope.type = data.data;
            }
        });
    };
    $scope.getNewsType();
    $scope.getXzqh = function () {
        $scope.loading = layer.load();
        $http.post('/common/GetXzqhByUser').success(function (data) {
            layer.close($scope.loading);
            if (data.data.length !== 0) {
                $scope.xzqh = data.data;
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.getXzqh();
    };
    $scope.reset();
});
app.controller('NewsCtrl', function ($scope, $http, $routeParams) {
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/home/News_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = [];
                data.data.forEach(function (e) {
                    e.time = Util.dateToYYMMDD(new Date(e.time));
                    $scope.data.push(e);
                });
                $scope.makePage(data);
            }
        });
    };
    $scope.getZwgk = function () {
        $scope.loading = layer.load();
        $http.post('/home/NewsCwgk_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.dataZwgk = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.getBjtj = function () {
        $scope.loading = layer.load();
        $http.post('/home/NewsBjtj_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.dataBjtj = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.getBjgs = function () {
        $scope.loading = layer.load();
        $http.post('/home/NewsBjgs_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.dataBjgs = [];
                data.data.forEach(function (e) {
                    e.FINISH_TIME = Util.dateToYYMMDD(new Date(e.FINISH_TIME));
                    $scope.dataBjgs.push(e);
                });
                $scope.makePage(data);
            }
        });
    };
    $scope.getBslc = function () {
        $scope.loading = layer.load();
        $http.post('/home/NewsBslc_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.dataBslc = data.data;
                $scope.makePage(data);
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
            layout: ['prev', 'page', 'next', 'count'],
            groups: 1,
            prev: '<<',
            next: '>>',
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    if ($scope.xzqh === '0') {
                        if ($scope.type === '1') {
                            $scope.getZwgk();
                        } else if ($scope.type === '2') {
                            $scope.getBjtj();
                        } else if ($scope.type === '3') {
                            $scope.getBjgs();
                        } else if ($scope.type === '4') {
                            $scope.getBslc();
                        } else {
                            console.log('没有对应分类');
                        }
                    } else {
                        $scope.search.string1 = $scope.xzqh;
                        $scope.search.string2 = $scope.type;
                        $scope.get();
                    }
                }
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = Util.getSearchObject();
        $scope.xzqh = $routeParams['xzqh'];
        $scope.type = $routeParams['type'];
        $scope.xzqhName = null;
        $scope.typeName = null;
        if ($scope.xzqh === '0') {
            $scope.xzqhName = '县级新闻';
            if ($scope.type === '1') {
                $scope.typeName = '政务公开';
                $scope.getZwgk();
            } else if ($scope.type === '2') {
                $scope.typeName = '办件统计';
                $scope.getBjtj();
            } else if ($scope.type === '3') {
                $scope.typeName = '办件公示';
                $scope.getBjgs();
            } else if ($scope.type === '4') {
                $scope.typeName = '办事流程';
                $scope.getBslc();
            } else {
                console.log('没有对应分类');
            }
        } else {
            $http.post('/common/GetXzqh_One', { code: $scope.xzqh }).success(function (data) {
                if (data.success) {
                    $scope.xzqhName = data.data.name;
                }
            });
            $http.post('/common/GetNewsType_One', { id: $scope.type }).success(function (data) {
                if (data.success) {
                    $scope.typeName = data.data.name;
                }
            });
            $scope.search.string1 = $scope.xzqh;
            $scope.search.string2 = $scope.type;
            $scope.get();
        }
    };
    $scope.reset();
});
app.controller('NewsDetailCtrl', function ($scope, $http, $routeParams, $sce) {
    $scope.timeStart = new Date();
    $scope.$on('$destroy', function () {
        $http.post('/common/Count_User_News_Duration', { duration: Math.floor(new Date().getTime() - $scope.timeStart.getTime()) / 1000 });
    });
    $scope.getNews = function () {
        $scope.loading = layer.load();
        $http.post('/home/NewsDetail_Get', { id: $scope.newsid }).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.news = data.data[0];
                $scope.news.html = $sce.trustAsHtml($scope.news.content);
            }
        });
    };
    $scope.get = function () {
        $http.post('/home/NewsTalk_GetList', { id: $scope.newsid }).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
            }
        });
    };
    $scope.add_show = function () {
        $scope.model = Util.getObject($scope.m);
        $scope.model.newsid = $scope.newsid;
        $('#modal').modal('toggle');
    };
    $scope.add = function () {
        if (Util.isNull($scope.model.question)) {
            layer.msg('提问为空');
            return;
        }
        var formData = new FormData();
        var files = $('#files')[0].files;
        for (var i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }
        formData.append('newsid', $scope.model.newsid);
        formData.append('question', $scope.model.question);
        $.ajax({
            url: '/home/NewsTalk_Add',
            type: 'post',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                layer.msg(data.message);
                if (data.success) {
                    $('#files').val('');
                    $('#modal').modal('toggle');
                    $scope.get();
                }
            }
        });
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
        $scope.newsid = $routeParams['id'];
        $scope.getNews();
        $scope.get();
    };
    $scope.reset();
});
app.controller('NewsDetailSpiderCtrl', function ($scope, $http, $routeParams, $sce) {
    $scope.getZwgk = function () {
        $scope.loading = layer.load();
        $http.post('/home/NewsDetailSpider_Zwgk_Get', { id: $scope.newsid }).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.newsZwgk = data.data[0];
                $scope.newsZwgk.html = $sce.trustAsHtml($scope.newsZwgk.content);
            }
        });
    };
    $scope.getBslc = function () {
        $scope.loading = layer.load();
        $http.post('/home/NewsDetailSpider_Bslc_Get', { id: $scope.newsid }).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.newsBslc = data.data[0];
                $scope.newsBslc.html = $sce.trustAsHtml($scope.newsBslc.content);
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.type = $routeParams['type'];
        $scope.newsid = $routeParams['id'];
        if ($scope.type === '1') {
            $scope.getZwgk();
        } else if ($scope.type === '4') {
            $scope.getBslc();
        } else {
            console.log('没有对应分类');
        }
    };
    $scope.reset();
});
app.controller('MessageCtrl', function ($scope, $http, $rootScope) {
    $scope.read = function () {
        $http.post('/home/Message_Read').success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $rootScope.getMessage();
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
    };
    $scope.reset();
});
app.controller('MyRankCtrl', function ($scope, $http, $routeParams) {
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/home/MyRank_GetList').success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                if ($scope.type === '1') {
                    $scope.typeName = '浏览量';
                    $scope.data = data.data.scan;
                    $scope.data.forEach(function (e) {
                        e.data = e.count_scan;
                    });
                } else if ($scope.type === '2') {
                    $scope.typeName = '提问数';
                    $scope.data = data.data.question;
                    $scope.data.forEach(function (e) {
                        e.data = e.count_question;
                    });
                } else if ($scope.type === '3') {
                    $scope.typeName = '浏览时长';
                    $scope.data = data.data.duration;
                    $scope.data.forEach(function (e) {
                        e.data = Util.secondToHHMMSS(e.count_duration);
                    });
                } else {
                    console.log('没有对应分类');
                }
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.type = $routeParams['type'];
        $scope.typeName = null;
        $scope.get();
    };
    $scope.reset();
});
app.controller('MyQuestionCtrl', function ($scope, $http, $routeParams) {
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/home/MyQuestion_GetListByPage', $scope.search).success(function (data) {
            layer.close($scope.loading);
            if (data.success) {
                $scope.data = data.data;
                $scope.makePage(data);
            }
        });
    };
    $scope.getFile = function (e) {
        $http.post('/admin/NewsTalk_GetFile', { id: e.id }).success(function (data) {
            if (data.success) {
                $scope.files = data.data;
            }
        });
        $('#modal').modal('toggle');
    };
    $scope.makePage = function (data) {
        layui.laypage.render({
            elem: 'page',
            count: data.count,
            curr: $scope.search.page,
            limit: $scope.search.limit,
            limits: [10, 20, 30, 40, 50],
            layout: ['prev', 'page', 'next', 'count'],
            groups: 1,
            prev: '<<',
            next: '>>',
            jump: function (obj, first) {
                $scope.search.page = obj.curr;
                $scope.search.limit = obj.limit;
                if (!first) {
                    $scope.get();
                }
            }
        });
    };
    $scope.reset = function () {
        $scope.loading = null;
        $scope.search = Util.getSearchObject();
        $scope.get();
    };
    $scope.reset();
});