 # include installed libs
export LD_LIBRARY_PATH=/usr/local/lib
# compile dll's
xbuild /p:Configuration=Release ./atom-sdk/atom-sdk.sln
- xbuild /p:Configuration=Release ./atom-sdk/test/atom-unity-test.sln
# copy debug information
cp ./atom-sdk/Temp/bin/Release/Assembly-CSharp-firstpass.dll.mdb ./atom-sdk/test/bin/Release/
cp ./atom-sdk/Temp/bin/Release/Assembly-CSharp.dll.mdb ./atom-sdk/test/bin/Release/
# create coverage file
mono --profile=monocov:outfile=monocovCoverage.cov,+Assembly-CSharp-firstpass ./testrunner/NUnit.Runners.2.6.4/tools/nunit-console.exe ./atom-sdk/test/bin/Release/test.dll
monocov --export-xml=monocovCoverage monocovCoverage.cov 
REPO_COMMIT_AUTHOR=$(git show -s --pretty=format:"%cn")
REPO_COMMIT_AUTHOR_EMAIL=$(git show -s --pretty=format:"%ce")
REPO_COMMIT_MESSAGE=$(git show -s --pretty=format:"%s")
mono ./testrunner/coveralls.net.0.6.0/tools/csmacnz.Coveralls.exe --monocov -i ./monocovCoverage --repoToken "bIBwmmSHpfkqtrJmE9a369DHCk9pUKyBu" --commitId $TRAVIS_COMMIT --commitBranch "$TRAVIS_BRANCH" --commitAuthor "$REPO_COMMIT_AUTHOR" --commitEmail "$REPO_COMMIT_AUTHOR_EMAIL" --commitMessage "$REPO_COMMIT_MESSAGE" --jobId $TRAVIS_JOB_ID --serviceName "travis-ci" --useRelativePaths