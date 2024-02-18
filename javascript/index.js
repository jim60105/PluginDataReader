window.indexJs = {
    noticeBoxInit: function () {
        //註冊滑鼠事件
        var noticeTitle = document.getElementById("noticeTitle");
        noticeTitle.addEventListener("mouseup", function () {
            window.indexJs.noticeBoxtoggleDisplay(noticeTitle.style.left=="0vw");
        }, false);
        window.indexJs.noticeBoxtoggleDisplay(true);
    },
    noticeBoxtoggleDisplay: function (open) {
        var viewWidth = 35; //vw
        var noticeBox = document.getElementById("noticeBox");
        var noticeTitle = document.getElementById("noticeTitle");

        if (open) {
            //開啟清單
            noticeBox.style.left = "0vw";
            noticeTitle.style.left = viewWidth + "vw";
        } else {
            //關閉清單
            noticeBox.style.left = "-" + viewWidth + "vw";
            noticeTitle.style.left = "0vw";
        }
    },
    parseJson: function (json, key) {
        var element = document.getElementById('div' + key);
        if (element) {
            element.appendChild(new JSONFormatter(JSON.parse(json), 3, {
                hoverPreviewEnabled: false,
                hoverPreviewArrayCount: 100,
                hoverPreviewFieldCount: 5,
                animateOpen: true,
                animateClose: true,
                theme: 'dark', // or 'dark'
                useToJSON: false // use the toJSON method to render an object as a string as available
            }).render());
        }
    }
};
