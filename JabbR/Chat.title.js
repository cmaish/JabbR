/// <reference path="Scripts/jquery-1.7.js" />
(function ($, tinycon) {
    "use strict";

    var title = {
        updateTitle: function (originalTitle, unread, isUnreadMessageForUser) {
            tinycon.setBubble(unread, undefined, { isUnreadMessageForUser: isUnreadMessageForUser });
        }
    };

    if (!window.chat) {
        window.chat = {};
    }
    window.chat.title = title;

    tinycon.setOptions({
        fallbackTitle: function (originalTitle, label, forced, additionalData) {
            var docTitle = '';
            
            if (label > 0) {
                docTitle = (additionalData.isUnreadMessageForUser ? '*' : '');
                if (forced) {
                    docTitle += '(' + label + ') ';
                }   
            }
            
            docTitle += originalTitle;
            return docTitle;
        },
        fallback: 'force'
    });
})(jQuery, window.Tinycon);