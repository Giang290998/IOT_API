#!/bin/bash

# Kiểm tra số lượng đối số
if [ "$#" -ne 2 ]; then
    echo "Usage: $0 <new_user> <new_password>"
    exit 1
fi

# Lấy new_user và new_password từ đối số
new_user="$1"
new_password="$2"

# Đường dẫn đến file password
password_file="/path/to/mosquitto/password_file"

# Tạo user và password mới
echo "$new_user:$new_password" >> $password_file

# Lấy PID của quy trình Mosquitto
mosquitto_pid=$(pgrep mosquitto)

# Kiểm tra xem Mosquitto đang chạy hay không
if [ -n "$mosquitto_pid" ]; then
    # Gửi tín hiệu SIGHUP để reload file mật khẩu
    kill -HUP $mosquitto_pid
    echo "Reloaded Mosquitto's password file."
else
    echo "Mosquitto is not running. Password file updated but not reloaded."
fi
