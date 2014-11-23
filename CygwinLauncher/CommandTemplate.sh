#!/bin/bash

[ -f /etc/profile ] && . /etc/profile
[ -f ~/.bash_profile ] && . ~/.bash_profile
[ -f ~/.bash_login ] && . ~/.bash_login
[ -f ./.profile ] && . ./.profile

cd "$(cygpath '${directory}')"
/bin/bash "$(cygpath '${filePath}')"