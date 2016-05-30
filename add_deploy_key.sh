#!/usr/bin/expect

spawn eval `ssh-agent -s`
spawn ssh-add deploy_key
expect "deploy_key:"
send "\n"

# done
expect eof