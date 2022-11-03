
$(function () {

    var sels = $(".fake-sel");
    
    var imgs_ = [
        [
            'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTjp0SaTG2i8f-S9SDJzPCIXDAHVsUXwltp6XGzSgqFwV6QNllyu38vvtDr_6rYTv6vnUQ&usqp=CAU',
            'https://media.istockphoto.com/photos/car-engine-picture-id520977101?k=20&m=520977101&s=612x612&w=0&h=hX9ZhYJSb5FUXhDpMirWJhTAtwOFDX1fixbKRRnfWtg=',
            'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTBzYiIlvpuDa1Yl9FxWMlhzM4_joBFtg5ZmQ&usqp=CAU',
            'https://cdn.leroymerlin.com.br/products/motor_eletrico_universal_para_aparador_de_grama_hc76_30_700w_1566984066_9f02_600x600.jpeg',
            'https://cemavel.com.br/site/wp-content/uploads/2018/09/um-perto-de-um-motor-07093634097042-300x229.png'
        ],
        [
            'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTjp0SaTG2i8f-S9SDJzPCIXDAHVsUXwltp6XGzSgqFwV6QNllyu38vvtDr_6rYTv6vnUQ&usqp=CAU',
            'https://media.istockphoto.com/photos/car-engine-picture-id520977101?k=20&m=520977101&s=612x612&w=0&h=hX9ZhYJSb5FUXhDpMirWJhTAtwOFDX1fixbKRRnfWtg=',
            'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTBzYiIlvpuDa1Yl9FxWMlhzM4_joBFtg5ZmQ&usqp=CAU',
            'https://cdn.leroymerlin.com.br/products/motor_eletrico_universal_para_aparador_de_grama_hc76_30_700w_1566984066_9f02_600x600.jpeg',
            'https://cemavel.com.br/site/wp-content/uploads/2018/09/um-perto-de-um-motor-07093634097042-300x229.png'
        ]
    ];

    sels.each(function (x) {

        var $t = $(this);

        var opts_ = '', first;

        $t.find("option").each(function (i) {

            if (i == 0) {
                first = "<li optionId='" + $(this).parent().attr("id") + "' value='" + $(this).val() + "'><img src='" + imgs_[x][i] + "'>" + $(this).text() + "</li>";
            }
            opts_ += "<li" + (i == 0 ? " class='ativo'" : '') + " optionId='" + $(this).parent().attr("id") + "' value='" + $(this).val() + "'><img src='" + imgs_[x][i] + "'>" + $(this).text() + "</li>";
        });

        $t
            .wrap("<div class='fake-sel-wrap'></div>")
            .hide()
            .parent()
            .css("width", $t.outerWidth() + 100)
            .append("<ul>" + first + opts_ + "</ul>")
            .find("ul")
            .on("click", function (e) {
                e.stopPropagation();
                $(this).toggleClass("ativo");
            })
            .find("li:not(:first)")
            .on("click", function () {
                $(this)
                    .addClass("ativo")
                    .siblings()
                    .removeClass("ativo")
                    .parent()
                    .find("li:first")
                    .html($(this).html());

                //selectElement($(this).attr("optionId"), $(this).attr("value"))
                //$("div.id_100 select").val($(this).attr("value"));
                //$("#select_id").val("val2").change();
                //var ID = $(this).attr("optionId");
                //$('#' + ID).val($(this).attr("value")).change();
                //$t.val($(this).text());

                $t.val($(this).attr("value"));

            });
    });

    $(document).on("click", function () {
        $(".fake-sel-wrap ul").removeClass("ativo");
    });
    

    function selectElement(id, valueToSelect) {

        alert(id);
        alert(valueToSelect);

        let element = document.getElementById(id);
        element.value = valueToSelect;

        alert('ok')
    }

});