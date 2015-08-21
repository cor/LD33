#!/bin/bash
echo Creating temp working directory
mkdir clone && cd clone

echo Cloning repo
git clone "http://${GH_REF}" .

echo Checking out gh-pages branch
git checkout gh-pages

echo 4a
# do gh-pages stuff here
branch=$TRAVIS_BRANCH

echo 4b
version=0.$TRAVIS_BUILD_NUMBER

echo 4c
timestamp=`date +"%Y-%m-%d %H:%M:%S"`

echo 4d
datestamp=`date +"%Y-%m-%d"`

echo 4e
template=../Scripts/yyyy-mm-dd-branch-version.md

echo 4f
filename=_posts/${datestamp}-${branch}-v${version}.md

echo 4g
echo ${branch}
echo ${version}
echo ${timestamp}
echo ${datestamp}
echo ${template}
echo ${filename}

sed -e 's/@version@/'"${version}"'/g' -e 's/@branch@/'"${branch}"'/g' -e 's/@timestamp@/'"${timestamp}"'/g' ${template} > ${filename}

echo 5
git add ${filename}

echo Commiting
git commit -m "Deploy to gh-pages"

echo Pushing to GitHub
git push --force --quiet "https://${GH_TOKEN}@${GH_REF}" > /dev/null 2>&1
