
//gráfico
$(function () {

    var tt = document.createElement('div'),
        leftOffset = -(~~$('html').css('padding-left').replace('px', '') + ~~$('body').css('margin-left').replace('px', '')),
        topOffset = -32;
    tt.className = 'tooltip top fade in';
    document.body.appendChild(tt);

    var data = [{
        "xScale": "ordinal", "comp": [], "main": [{
            "className": ".main.l1", "data": [{ "y": 15, "x": "2012-11-19T00:00:00" }
                                            , { "y": 11, "x": "2012-11-20T00:00:00" }
                                            , { "y": 8, "x": "2012-11-21T00:00:00" }
                                            , { "y": 10, "x": "2012-11-22T00:00:00" }
                                            , { "y": 1, "x": "2012-11-23T00:00:00" }
                                            , { "y": 6, "x": "2012-11-24T00:00:00" }
                                            , { "y": 8, "x": "2012-11-25T00:00:00" }
            ]
        }
                                            , {
                                                "className": ".main.l2", "data": [{ "y": 29, "x": "2012-11-19T00:00:00" }
                                                                                , { "y": 33, "x": "2012-11-20T00:00:00" }
                                                                                , { "y": 13, "x": "2012-11-21T00:00:00" }
                                                                                , { "y": 16, "x": "2012-11-22T00:00:00" }
                                                                                , { "y": 7, "x": "2012-11-23T00:00:00" }
                                                                                , { "y": 18, "x": "2012-11-24T00:00:00" }
                                                                                , { "y": 8, "x": "2012-11-25T00:00:00" }
                                                ]
                                            }
        ], "type": "line-dotted", "yScale": "linear"
    }
                , {
                    "xScale": "ordinal", "comp": [], "main": [{
                        "className": ".main.l1", "data": [{ "y": 12, "x": "2012-11-19T00:00:00" }
                                                        , { "y": 18, "x": "2012-11-20T00:00:00" }
                                                        , { "y": 8, "x": "2012-11-21T00:00:00" }
                                                        , { "y": 7, "x": "2012-11-22T00:00:00" }
                                                        , { "y": 6, "x": "2012-11-23T00:00:00" }
                                                        , { "y": 12, "x": "2012-11-24T00:00:00" }
                                                        , { "y": 8, "x": "2012-11-25T00:00:00" }
                        ]
                    }
                                                        , {
                                                            "className": ".main.l2", "data": [{ "y": 29, "x": "2012-11-19T00:00:00" }
                                                                                            , { "y": 33, "x": "2012-11-20T00:00:00" }
                                                                                            , { "y": 13, "x": "2012-11-21T00:00:00" }
                                                                                            , { "y": 16, "x": "2012-11-22T00:00:00" }
                                                                                            , { "y": 7, "x": "2012-11-23T00:00:00" }
                                                                                            , { "y": 18, "x": "2012-11-24T00:00:00" }
                                                                                            , { "y": 8, "x": "2012-11-25T00:00:00" }
                                                            ]
                                                        }
                    ], "type": "cumulative", "yScale": "linear"
                }
                , {
                    "xScale": "ordinal", "comp": [], "main": [{
                        "className": ".main.l1", "data": [{ "y": 12, "x": "2012-11-19T00:00:00" }
                                                        , { "y": 18, "x": "2012-11-20T00:00:00" }
                                                        , { "y": 8, "x": "2012-11-21T00:00:00" }
                                                        , { "y": 7, "x": "2012-11-22T00:00:00" }
                                                        , { "y": 6, "x": "2012-11-23T00:00:00" }
                                                        , { "y": 12, "x": "2012-11-24T00:00:00" }
                                                        , { "y": 8, "x": "2012-11-25T00:00:00" }
                        ]
                    }
                                                        , {
                                                            "className": ".main.l2", "data": [{ "y": 29, "x": "2012-11-19T00:00:00" }
                                                                                            , { "y": 33, "x": "2012-11-20T00:00:00" }
                                                                                            , { "y": 13, "x": "2012-11-21T00:00:00" }
                                                                                            , { "y": 16, "x": "2012-11-22T00:00:00" }
                                                                                            , { "y": 7, "x": "2012-11-23T00:00:00" }
                                                                                            , { "y": 18, "x": "2012-11-24T00:00:00" }
                                                                                            , { "y": 8, "x": "2012-11-25T00:00:00" }
                                                            ]
                                                        }
                    ], "type": "bar", "yScale": "linear"
                }
    ];
    var order = [0, 1, 0, 2],
        i = 0,
        xFormat = d3.time.format('%A'),
        chart = new xChart('line-dotted', data[order[i]], '#chart-example', {
            axisPaddingTop: 5,
            dataFormatX: function (x) {
                return new Date(x);
            }
        ,
            tickFormatX: function (x) {
                return xFormat(x);
            }
        ,
            mouseover: function (d, i) {
                var pos = $(this).offset();
                $(tt).html('<div class="arrow"></div><div class="tooltip-inner">' + d3.time.format('%A')(d.x) + ': ' + d.y + '</div>')
                .css({
                    top: topOffset + pos.top, left: pos.left + leftOffset
                }
                    )
                .show();
            }
        ,
            mouseout: function (x) {
                $(tt).hide();
            }
        ,
            timing: 1250
        }
                        ),
        rotateTimer,
        toggles = d3.selectAll('#upd-chart a'),
        t = 3500;

    function updateChart(i) {
        var d = data[i];
        chart.setData(d);
        toggles.classed('active', function () {
            return (d3.select(this).attr('data-type') === d.type);
        }
                        );
        return d;
    }

    toggles.on('click', function (d, i) {
        clearTimeout(rotateTimer);
        updateChart(i);
    }
            );

    function rotateChart() {
        i += 1;
        i = (i >= order.length) ? 0 : i;
        var d = updateChart(order[i]);
        rotateTimer = setTimeout(rotateChart, t);
    }
    rotateTimer = setTimeout(rotateChart, t);
}
());


var carrossel = {
    firstItem: 0,
    totalItens: 0,
    itensShow : 3,

    countItens: function () {
        this.totalItens = $(".carrosselItem").length;
    },

    getLargItem: function () {
        var largUtil = $("#page-content-wrapper").width();
        return parseInt(largUtil / this.itensShow);
    },

    setLargura: function () {
        var larguraItem = this.getLargItem();

        var w1 = larguraItem * this.itensShow, //largura do wrapper
            w2 = larguraItem * this.totalItens,
            w3 = 100 / this.totalItens;

        $(".carrossel").width(w1);
        $(".carousel-wrapper").width(w2);
        $(".carrosselItem").width(w3 + "%");
    },

    moveNext: function () {
        $(".carousel-wrapper").animate({
            left: "-=" + this.getLargItem() + "px"
        }, 500);
        this.firstItem++;

        if ((this.totalItens - this.itensShow) <= this.firstItem) {
            $(".next").addClass("disabled");
        }
    },
    movePrevious: function () {
        $(".carousel-wrapper").animate({
            left: "+=" + this.getLargItem() + "px"
        }, 500);
        this.firstItem--;
        
        if (this.firstItem < 1) {
            $(".previous").addClass("disabled");
        }
    },

    setAntProx: function () {
        var that = this;
        $(".previous").on('click', function () {

            if (that.firstItem > 0) {
                $(".carrossel .next").removeClass("disabled");
                that.movePrevious();
            }
            else {
                $(".carrossel .previous").addClass("disabled");
            }
        });

        $(".next").on('click', function () {

            if ((that.totalItens - that.itensShow) >= that.firstItem ) {
                $(".carrossel .previous").removeClass("disabled");
                that.moveNext();
            }
            else {
                $(".carrossel .next").addClass("disabled");
            }
        });
    },
    onResize: function () {
        this.setLargura();
        $(".carousel-wrapper").css("left", this.firstItem * this.getLargItem() * (-1)); //para corrigir posicionamento 
    },

    loadConteudo : function(pJson){  
        console.log('ok');
        var hold = $(".carousel-wrapper"),
             html = "";
    
        $(pJson).each(function(index){
    
            var htmlItem = "";
            
            htmlItem = '<div class="carrosselItem">';
            htmlItem += ' <a href="#" class="bt-close glyph-icon icon-remove"></a>';
            htmlItem += '    <div class="col-md-2">';
            htmlItem += '        <div class="tile-box tile-box-alt bg-' + pJson[index].cor + '">';
            htmlItem += '            <div class="tile-header">' + pJson[index].nome + '</div>';
            htmlItem += '            <div class="tile-content-wrapper">';
            htmlItem += '                <i class="glyph-icon">' + pJson[index].logados + ' logged</i>';
            htmlItem += '                <small>last message</small>';
            htmlItem += '                <strong>' + pJson[index].hora + '</strong>';
            htmlItem += '            </div>';
            htmlItem += '        </div>';
            htmlItem += '    </div>';
            htmlItem += '</div>';
      
            html += htmlItem;
      
        });
    
        hold.append(html);
    
        this.config();

        $(".bt-close").on('click', function () {
            carrossel.close(this);
        });


    },

    config: function () {

        this.countItens();

        setTimeout(function () {
            var altura = $(".carrosselItem").height();
            $(".carrossel, .carousel-wrapper").height(altura);

        }, 500);

        this.setLargura();

        this.setAntProx();
        $(".previous").addClass("disabled");

    },

    close: function (e) {
        var elementoRemovido = $(e).parents(".carrosselItem").index(),
            that = this;

        $(e).parents(".carrosselItem").fadeOut(function () {
            $(this).remove();

            that.countItens();
            that.setLargura();

            if (that.firstItem > 0 && ((that.totalItens - that.firstItem) < that.itensShow)) {// move para direita se tiver mais do que o número de itens mostrado
                that.movePrevious();
            }

            if ((that.totalItens - that.itensShow) <= that.firstItem) {
                $(".next").addClass("disabled");
            }
        });
    }
};
