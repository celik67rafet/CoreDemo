﻿using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class Message2Manager : IMessage2Service
    {
        IMessage2Dal _message2Dal;
        public Message2Manager( IMessage2Dal message2Dal )
        {
            _message2Dal = message2Dal;
        }

        public List<Message2> TGetInboxListByWriter(int id)
        {
            return _message2Dal.GetInboxListByWriter( id );
        }

        public List<Message2> GetList()
        {
            return _message2Dal.GetListAll();
        }

        public void TAdd(Message2 t)
        {
            _message2Dal.Insert(t);
        }

        public void TDelete(Message2 t)
        {
            _message2Dal.Delete(t);
        }

        public Message2 TGetById(int id)
        {
            return _message2Dal.GetByID(id);
        }

        public void TUpdate(Message2 t)
        {
            _message2Dal.Update(t);
        }

        public List<Message2> TGetSendBoxListByWriter(int id)
        {
            return _message2Dal.GetSendBoxListByWriter(id);
        }
    }
}
