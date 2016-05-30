#!/usr/bin/expect

spawn ssh-add deploy_key
expect "deploy_key:"
send "\n"

# done
expect eof