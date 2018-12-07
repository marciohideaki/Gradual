(function(A)
{
    A.extend(A.fn,
    {
        pstrength: function(B)
        {
            var B = A.extend({ verdects: [ "muito fraca", "fraca", "forte", "muito forte" ]
                             , colors  : ["#FF0000", "#FFFF00", "#0000FF", "#32CD32" ]
                             , scores  : [10, 15, 30, 40 ]
                             , common  :["password", "Gradual", "gradual", "qwerty", "123456", "123", "102030", "123123", "123456", "123456", "456789", "789789" ]
                             , minchar : 5
                             }, B);

            return this.each(function()
            {
                var C = A(this).attr("id");
                //A(this).after("<div class=\"pstrength-minchar\" id=\"" + C + "_minchar\">O n&uacute;mero m&iacute;nimo de caracteres &eacute;: " + B.minchar + "</div>");
                A(this).after("<div class=\"pstrength-info\" id=\""    + C + "_text\"></div>");
                A(this).after("<div class=\"pstrength-bar\" id=\""     + C + "_bar\" style=\"border: 1px solid white; font-size: 1px; height: 10px; width: 0px;\"></div>");
                A(this).keyup(function() { A.fn.runPassword(A(this).val(), C, B) });
            })
        },

        runPassword: function(D, F, C)
        {
            nPerc = A.fn.checkPassword(D, C);

            var B = "#" + F + "_bar";
            var E = "#" + F + "_text";
            
            $(B).attr("title", null);

            if (nPerc <= C.scores[0])
            {
                strColor = C.colors[0];
                strText = C.verdects[0];
                $(B).attr("title", "Senha " + C.verdects[0])
                    .css( { backgroundPosition: "center -3px" } );

            }
            else if (nPerc > C.scores[0] && nPerc <= C.scores[1])
            {
                strColor = C.colors[1];
                strText = C.verdects[1];
                $(B).attr("title", "Senha " + C.verdects[1])
                    .css( { backgroundPosition: "center -39px" } );

            }
            else if (nPerc > C.scores[1] && nPerc <= C.scores[2])
            {
                strColor = C.colors[2];
                strText = C.verdects[2];
                $(B).attr("title", "Senha " + C.verdects[2])
                    .css( { backgroundPosition: "center -75px" } );

            }
            else
            {
                strColor = C.colors[3];
                strText = C.verdects[3];
                $(B).attr("title", "Senha " + C.verdects[3])
                    .css( { backgroundPosition: "center -111px" } );

            }

            A(B).css({backgroundColor:strColor});
            A(E).html("<span style='color: " + strColor + ";'>" + strText + "</span>")},

            checkPassword: function(C, B)
            {
                var F = 0;
                var E = B.verdects[0];

                if (C.length < B.minchar)
                {
                    F = (F -100);
                }
                else  if (C.length >= B.minchar && C.length <= (B.minchar + 2))
                {
                    F = (F + 6);
                }
                else if (C.length >= (B.minchar + 3) && C.length <= (B.minchar + 4))
                {
                    F = (F + 12);
                }
                else if (C.length >= (B.minchar + 5))
                {
                    F = (F + 18);
                }

                if (C.match(/[a-z]/))
                {
                    F = (F + 1);
                }
                if (C.match(/[A-Z]/))
                {
                    F = (F + 5);
                }
                if (C.match(/\d+/))
                {
                    F = (F + 5);
                }
                if (C.match(/(.*[0-9].*[0-9].*[0-9])/))
                {
                    F = (F + 7);
                }
                if (C.match(/.[!,@,#,$,%,^,&,*,?,_,~]/))
                {
                    F = (F + 5);
                }
                if (C.match(/(.*[!,@,#,$,%,^,&,*,?,_,~].*[!,@,#,$,%,^,&,*,?,_,~])/))
                {
                    F = (F + 7);
                }
                if (C.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/))
                {
                    F = (F + 2);
                }
                if (C.match(/([a-zA-Z])/) && C.match(/([0-9])/))
                {
                    F = (F + 3);
                }
                if (C.match(/([a-zA-Z0-9].*[!,@,#,$,%,^,&,*,?,_,~])|([!,@,#,$,%,^,&,*,?,_,~].*[a-zA-Z0-9])/))
                {
                    F = (F + 3);
                }

                for (var D = 0; D < B.common.length; D++)
                {
                    if (C.toLowerCase() == B.common[D])
                    {
                        F = -200;
                    }
                }

                return F;
            }
    })
})
(jQuery)