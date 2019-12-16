# MLNETModelBuilder

----- I have only tested this on ubuntu 18.04. -------


-This app will build your ml.net models.

-Uses automl and microsoft.ml.

-You will need to install .net core / mlnet before running this.
Dotnet Core -- https://dotnet.microsoft.com/download
mlnet cli -- dotnet tool install -g mlnet

-uses .csv/.tsv files. 

-all of the command line params can be set via gui tool.

-starts build process in a seperate console allowing you to train multiple instances.

++++++++ADDED CONVERT JSON TO TSV++++++++++++++++++

--click "Convert Json", 
-- fill out form,
-- press convert,
-- produces .tsv file in specified output folder.

NOTE+++++++++++++++++++++++++++++++++++++++++++++

when installing mlnet cli you need to use the following commands to set the environment variables. 
ubuntu 18.04 isnt setting them up right.

-- export PATH="$PATH:~/.dotnet/tools"

-- echo 'export PATH="$PATH:~/.dotnet/tools"' >> ~/.bashrc

+++++++++++++++++++++++++++++++++++++++++++++++++

INSTALL MONODEVELOP++++++++++++++++++++++++++++++

sudo apt install apt-transport-https dirmngr

sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

echo "deb https://download.mono-project.com/repo/ubuntu vs-bionic main" | sudo tee /etc/apt/sources.list.d/mono-official-vs.list

sudo apt update

sudo apt-get install monodevelop

+++++++++++++++++++++++++++++++++++++++++++++++++++++
