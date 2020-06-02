var app = angular.module('app', ['ngRoute']);
app.config(function ($routeProvider) {
    $routeProvider
        .when('/newstype', {
            templateUrl: '/home/newstype',
            controller: 'NewsTypeCtrl'
        })
        .when('/news/:xzqh/:type', {
            templateUrl: '/home/news',
            controller: 'NewsCtrl'
        })
        .when('/newsdetail/:id', {
            templateUrl: '/home/newsdetail',
            controller: 'NewsDetailCtrl'
        })
        .when('/newsdetailspider/:type/:id', {
            templateUrl: '/home/newsdetailspider',
            controller: 'NewsDetailSpiderCtrl'
        })
        .when('/message', {
            templateUrl: '/home/message',
            controller: 'MessageCtrl'
        })
        .when('/my', {
            templateUrl: '/home/my',
            controller: 'MyCtrl'
        })
        .when('/myrank/:type', {
            templateUrl: '/home/myrank',
            controller: 'MyRankCtrl'
        })
        .when('/myxzqh', {
            templateUrl: '/home/myxzqh',
            controller: 'MyXzqhCtrl'
        })
        .when('/myquestion', {
            templateUrl: '/home/myquestion',
            controller: 'MyQuestionCtrl'
        })
        .otherwise({
            redirectTo: '/newstype'
        });
});
app.controller('IndexCtrl', function ($http, $rootScope) {
    $http.post('/common/GetXzqhByUser').success(function (data) {
        if (data.success) {
            if (data.data.length === 0) {
                window.location.href = '#/myxzqh';
            }
        }
    });
    $rootScope.getMessage = function () {
        $http.post('/home/Message_GetList').success(function (data) {
            if (data.success) {
                data.data.forEach(function (e) {
                    e.time = window.Util.dateToYYMMDD(new Date(e.time));
                });
                $rootScope.message = data;
            }
        });
    };
    $rootScope.getMessage();
    $rootScope.back = function () {
        window.history.back();
    };
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
                    e.time = window.Util.dateToYYMMDD(new Date(e.time));
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
                    e.FINISH_TIME = window.Util.dateToYYMMDD(new Date(e.FINISH_TIME));
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
        $scope.search = window.Util.getSearchObject();
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
        $scope.model = window.Util.getObject($scope.m);
        $scope.model.newsid = $scope.newsid;
        $('#modal').modal('toggle');
    };
    $scope.add = function () {
        if (window.Util.isNull($scope.model.question)) {
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
app.controller('NewsDetailForDataVCtrl', function ($scope, $http, $sce) {
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
    $scope.reset = function () {
        $scope.loading = null;
        $scope.newsid = window.Util.getQueryVariable('id');
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
app.controller('MyXzqhCtrl', function ($scope, $http) {
    $scope.getXzqh = function () {
        $http.post('/common/GetXzqh').success(function (data) {
            $scope.xzqh = [];
            $scope.xzqh = data.data;
            $scope.get();
        });
    };
    $scope.getXzqh();
    $scope.get = function () {
        $scope.loading = layer.load();
        $http.post('/common/GetXzqhByUser').success(function (data) {
            layer.close($scope.loading);
            if (data.data.length !== 0) {
                $scope.xzqh.forEach(function (e) {
                    if (data.data.filter(function (f) { return f.code === e.code; }).length > 0) {
                        e.select = true;
                    } else {
                        e.select = false;
                    }
                });
            }
        });
    };
    $scope.add = function () {
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
        $http.post('/home/MyXzqh_Add', list).success(function (data) {
            layer.msg(data.message);
            if (data.success) {
                $scope.get();
            }
        });
    };
    $scope.check = function (e) {
        e.select = !e.select;
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
                        e.data = window.Util.secondToHHMMSS(e.count_duration);
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
app.controller('MyQuestionCtrl', function ($scope, $http) {
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
        $scope.search = window.Util.getSearchObject();
        $scope.get();
    };
    $scope.reset();
});