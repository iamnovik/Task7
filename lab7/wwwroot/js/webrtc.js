// Client-side (JavaScript with WebRTC and SignalR)
let connection = new signalR.HubConnectionBuilder().withUrl("/webrtcHub").build();

connection.on("Receive", (message) => {
    let msg = JSON.parse(message);

    // Если это SDP
    if (msg.type === 'offer' || msg.type === 'answer') {
        pc.setRemoteDescription(new RTCSessionDescription(msg))
            .then(() => {
                // Если это предложение, мы должны создать ответ
                if (msg.type === 'offer') {
                    pc.createAnswer().then((answer) => {
                        pc.setLocalDescription(answer);
                        connection.invoke("Send", JSON.stringify(answer));
                    });
                }
            });
    }
    // Если это ICE кандидат
    else if (msg.candidate) {
        pc.addIceCandidate(new RTCIceCandidate(msg.candidate));
    }
});

connection.start();

let pc = new RTCPeerConnection();

// Send any ICE candidates to the other peer
pc.onicecandidate = ({candidate}) => {
    connection.invoke("Send", JSON.stringify({ candidate }));
};

// Let's say we have a video element with the id "remoteVideo" for displaying the remote video stream
let remoteVideo = document.getElementById("remoteVideo");

pc.ontrack = (event) => {
    // Set the source of the video element to the stream received from the other peer
    remoteVideo.srcObject = event.streams[0];
};

// Получаем локальный медиапоток
navigator.mediaDevices.getUserMedia({ video: true, audio: true })
    .then(stream => {
        let localStream = stream;
        let localVideo = document.getElementById("localVideo");

        // Установите источник видеоэлемента на полученный поток
        localVideo.srcObject = stream;
        localStream.getTracks().forEach((track) => {
            // Add our video/audio tracks to the connection
            pc.addTrack(track, localStream);
        });

        // Create and send an offer to the other peer
        pc.createOffer().then((offer) => {
            pc.setLocalDescription(offer);
            connection.invoke("Send", JSON.stringify(offer));
        });
    })
    .catch(error => {
        console.error('Error accessing media devices.', error);
    });
