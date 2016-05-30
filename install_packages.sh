nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory testrunner
nuget install coveralls.net -Version 0.6.0 -OutputDirectory testrunner

# install doxygen
sudo apt-get install gtk-sharp2
sudo apt-get install expect
#sudo apt-get install doxygen
sudo apt-get --no-install-recommends install doxygen

travis encrypt-file deploy_key
echo "Deploy key: ${ENCRYPTION_LABEL}"

# install mono cecil
curl -sS https://api.nuget.org/packages/mono.cecil.0.9.5.4.nupkg > /tmp/mono.cecil.0.9.5.4.nupkg.zip
unzip /tmp/mono.cecil.0.9.5.4.nupkg.zip -d /tmp/cecil
cp /tmp/cecil/lib/net40/Mono.Cecil.dll .
cp /tmp/cecil/lib/net40/Mono.Cecil.dll /tmp/cecil/
# install monocov
git clone --depth=50 git://github.com/csMACnz/monocov.git ../../csMACnz/monocov
cd ../../csMACnz/monocov
cp /tmp/cecil/Mono.Cecil.dll .
./configure
make
sudo make install
# return to source directory
cd ../../ironSource/atom-unity

chmod 600 ../../deploy_key
eval `ssh-agent -s`