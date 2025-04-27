#!/bin/bash
cd src
make build-development
cd Wjt.Ui
make build-development
make start-development
cd ..
make start-development
cd ..
