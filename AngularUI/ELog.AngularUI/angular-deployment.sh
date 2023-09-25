#!/bin/bash

echo "Started"
DockerUsername=$1
DockerPassword=$2
BuildTag=$3
dockerRepoUrl=$4
dockerRepoName=$5

echo $DockerUsername
echo $DockerPassword

secret=`kubectl get secrets | awk '$1=="splatformk8sdockerauth" {print $1}'`
if [ "$secret" == "" ]; then
    echo 'Creating new secret for Docker Hub'
    kubectl create secret docker-registry splatformk8sdockerauth --docker-server=https://index.docker.io/v1/ --docker-username=$DockerUsername --docker-password=$DockerPassword --docker-email=bhushan.panhale@saviantconsulting.com
fi

#Replace Token with latest docker tag
echo "Replacing Image tag with - $BuildTag"
sed -i'' "s|#{dockerRepoUrl}#|$dockerRepoUrl|g" 'splatform-angular-deployment.yaml'
sed -i'' "s|#{dockerRepoName}#|$dockerRepoName|g" 'splatform-angular-deployment.yaml'
sed -i'' "s|#{Build.BuildId}#|$BuildTag|g" 'splatform-angular-deployment.yaml'

#sed -E -i'' "s/(.*splatformrepo:).*/\1$BuildTag/" 'splatform-angular-deployment.yaml' 

kubectl apply -f splatform-angular-deployment.yaml

#check if service is already exposed or not
#service=`kubectl get services | awk '$1=="splatform-angular-deployment" {print $1}'`
#if [ "$service" == "" ]; then
#    echo 'Exposing new service for Splatform Angular deployment'
#    kubectl expose deployment/splatform-angular-deployment --type=NodePort
#fi

echo "End"
