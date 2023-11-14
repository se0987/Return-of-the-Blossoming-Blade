import './App.css';
import blossoming from "./assets/blossoming.png"
import React from 'react';
import styled from 'styled-components'
import { Link } from 'react-router-dom';

function App() {
  const url = "http://k9d107.p.ssafy.io/downloads/test.jpg"
  return (
      <ImgContainer>
        <img alt="main_image" src={blossoming}/>
        <div>화산귀환</div>
        <p>알려지지 않은 이야기</p>
        <h1 onClick={()=>{window.open(url)}}>다운로드</h1>
      </ImgContainer>
  );
}

export default App;

const ImgContainer = styled.div`
  width: 100%;
  & > img {
    width: 100%;
    position: relative;
    pointer-events: none;
  }
  & > div{
    font-size: 70px;
    position: absolute;
    top: 101%;
    left: 50%;
    transform: translate( -50%, -50% );
    color: black;
    font-family: 'kdg_Medium';
    src: url('https://cdn.jsdelivr.net/gh/projectnoonnu/noonfonts-20-12@1.0/kdg_Medium.woff') format('woff');
    font-weight: normal;
    font-style: normal;
    pointer-events: none;
  }

  & > p{
    font-size: 60px;
    position: absolute;
    top: 102%;
    left: 50%;
    transform: translate( -50%, -50% );
    color: black;
    font-family: 'kdg_Medium';
    src: url('https://cdn.jsdelivr.net/gh/projectnoonnu/noonfonts-20-12@1.0/kdg_Medium.woff') format('woff');
    font-weight: normal;
    font-style: normal;
    pointer-events: none;
  }
  & > button{
    font-size: 50px;
    position: absolute;
    top: 103%;
    left: 50%;
    transform: translate( -50%, -50% );
    color: black;
    background-color: none;
  }

  & > h1{
    font-size: 60px;
    position: absolute;
    top: 120%;
    left: 50%;
    transform: translate( -50%, -50% );
    color: black;
    font-family: 'kdg_Medium';
    src: url('https://cdn.jsdelivr.net/gh/projectnoonnu/noonfonts-20-12@1.0/kdg_Medium.woff') format('woff');
    font-weight: normal;
    font-style: normal;
    cursor: pointer;
    user-select: none;
  }
  @font-face {
    font-family: 'kdg_Medium';
    src: url('https://cdn.jsdelivr.net/gh/projectnoonnu/noonfonts-20-12@1.0/kdg_Medium.woff') format('woff');
    font-weight: normal;
    font-style: normal;
}
`
