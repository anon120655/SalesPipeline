
//new MutationObserver((mutations, observer) => {
//	if (document.querySelector('#components-reconnect-modal h5 a')) {
//		async function Reconnection() {
//			await fetch('');
//			location.reload();
//		}
//		observer.disconnect();
//		Reconnection();
//		setInterval(Reconnection, 1000);
//	}
//}).observe(document.body, { childList: true, subtree: true });


var _baseUriApis = $('#baseUriApi').val()
console.log('_baseUriApis', _baseUriApis)

new MutationObserver((mutations, observer) => {
    if (document.querySelector('#components-reconnect-modal h5 a')) {
        // เพิ่ม style เพื่อซ่อน modal
        const style = document.createElement('style');
        style.textContent = `
            #components-reconnect-modal {
                display: none !important;
            }

            @keyframes slideIn {
                from { transform: translateX(100%); opacity: 0; }
                to { transform: translateX(0); opacity: 1; }
            }
        `;
        document.head.appendChild(style);

        // ฟังก์ชั่นสร้างและแสดง notification
        const showNotification = () => {
            // ลบ notification เก่าถ้ามี
            const existingNotification = document.getElementById('update-notification');
            if (existingNotification) {
                return; // ถ้ามี notification อยู่แล้ว ไม่ต้องสร้างใหม่
            }

            // สร้าง container สำหรับ notification
            const notificationContainer = document.createElement('div');
            notificationContainer.id = 'update-notification';
            notificationContainer.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                background: #ffffff;
                border: 2px solid #2B446C;
                padding: 16px 20px;
                border-radius: 8px;
                z-index: 999999;
                box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                font-family: system-ui, -apple-system, sans-serif;
                max-width: 400px;
                animation: slideIn 0.5s ease-out;
            `;

            // เนื้อหาของ notification
            notificationContainer.innerHTML = `
                <div style="display: flex; align-items: start; gap: 12px;">
                    <div style="flex-grow: 1;">
                        <div style="font-weight: bold; color: #2B446C; margin-bottom: 4px;">
                            ระบบขาดการเชื่อมต่อ
                        </div>
                        <div style="color: #334155; font-size: 14px;">
                            กรุณารีเฟรชหน้าเว็บและลองใช้งานอีกครั้ง
                        </div>
                    </div>
                    <button onclick="location.reload()" style="
                        background: #2B446C;
                        color: white;
                        border: none;
                        padding: 8px 12px;
                        border-radius: 4px;
                        cursor: pointer;
                        font-size: 14px;
                        transition: background-color 0.2s;
                    ">
                        รีเฟรช
                    </button>
                </div>
            `;

            // แทรก notification เข้าไปใน document
            document.body.appendChild(notificationContainer);
        };

        // เช็ค server และแสดง notification
        async function checkServer() {
            //try {
            //    await fetch(''); // เช็คว่า server กลับมาแล้ว
            //    showNotification(); // แสดง notification เมื่อ server กลับมา
            //} catch (error) {
            //    console.log('Server still unavailable');
            //}
            try {
                const response = await fetch(`${_baseUriApis}/v1/Security/GetReconnect`);
                if (response.ok) {
                    showNotification(); // แสดง notification เมื่อ server กลับมา
                    //location.reload();
                }
            } catch (e) {
                console.log('Server still unavailable...');
            }
        }

        observer.disconnect();
        checkServer();
        // เช็ค server ทุก 5 วินาที
        setInterval(checkServer, 5000);
    }
}).observe(document.body, { childList: true, subtree: true });


