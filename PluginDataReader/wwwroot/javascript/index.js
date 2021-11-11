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
    }
};

function dataURItoBlob(dataURI, scale = -1) {
    return new Promise(function (resolve, reject) {
        if (scale > 0) {
            var canvas = document.createElement('canvas'),
                context = canvas.getContext('2d'),
                image = new Image();
            image.addEventListener('load', function () {
                context.save();
                canvas.width = image.width;
                canvas.height = image.height;
                context.drawImage(image, 0, 0, Math.round(image.width * scale), Math.round(image.height * scale));
                context.restore();
                dataURI = canvas.toDataURL();
                URL.revokeObjectURL(image.src);
                resolve();
                //resolve(context.getImageData(0, 0, canvas.width, canvas.height));
            }, false);
            image.src = dataURI;
        } else { resolve(); }
    }).then(function () {
        // convert base64/URLEncoded data component to raw binary data held in a string
        var byteString;
        if (dataURI.split(',')[0].indexOf('base64') >= 0)
            byteString = atob(dataURI.split(',')[1]);
        else
            byteString = unescape(dataURI.split(',')[1]);

        // separate out the mime component
        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

        // write the bytes of the string to a typed array
        var ia = new Uint8Array(byteString.length);
        for (var i = 0; i < byteString.length; i++) {
            ia[i] = byteString.charCodeAt(i);
        }

        return (new Blob([ia], { type: mimeString }));
    });
}
