<%@ Page Title="ChappalSoft : Home" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">   
    <div class="main-panel">
        <div class="content-wrapper pb-0" style="min-height:570px;">
            <div class="page-header flex-wrap">
                <h3 class="mb-0">Welcome to ChappalSoft! <span class="pl-0 h6 pl-sm-2 text-muted d-inline-block">a software by AzkoIT.</span>
                </h3>
            </div>
            <div class="row">
              <div class="col-xl-3 col-lg-12 stretch-card grid-margin">
                <div class="row">
                  <div class="col-xl-12 col-md-6 stretch-card grid-margin grid-margin-sm-0 pb-sm-3">
                    <div class="card bg-warning">
                      <div class="card-body px-3 py-4">
                        <div class="d-flex justify-content-between align-items-start">
                          <div class="color-card">
                            <p class="mb-0 color-card-head">Sales</p>
                            <h2 class="text-white">1,50,678.<span class="h5">00</span>
                            </h2>
                          </div>
                          <i class="card-icon-indicator mdi mdi-basket bg-inverse-icon-warning"></i>
                        </div>
                        <h6 class="text-white">This month</h6>
                      </div>
                    </div>
                  </div>
                  <div class="col-xl-12 col-md-6 stretch-card grid-margin grid-margin-sm-0 pb-sm-3">
                    <div class="card bg-danger">
                      <div class="card-body px-3 py-4">
                        <div class="d-flex justify-content-between align-items-start">
                          <div class="color-card">
                            <p class="mb-0 color-card-head">Purchase</p>
                            <h2 class="text-white"> 5,300.<span class="h5">00</span>
                            </h2>
                          </div>
                          <i class="card-icon-indicator mdi mdi-cube-outline bg-inverse-icon-danger"></i>
                        </div>
                        <h6 class="text-white">This month</h6>
                      </div>
                    </div>
                  </div>
                  <div class="col-xl-12 col-md-6 stretch-card grid-margin grid-margin-sm-0 pb-sm-3 pb-lg-0 pb-xl-3">
                    <div class="card bg-primary">
                      <div class="card-body px-3 py-4">
                        <div class="d-flex justify-content-between align-items-start">
                          <div class="color-card">
                            <p class="mb-0 color-card-head">Expenses</p>
                            <h2 class="text-white"> 1,753.<span class="h5">00</span>
                            </h2>
                          </div>
                          <i class="card-icon-indicator mdi mdi-briefcase-outline bg-inverse-icon-primary"></i>
                        </div>
                        <h6 class="text-white">This month</h6>
                      </div>
                    </div>
                  </div>                  
                </div>
              </div>
              <div class="col-xl-9 stretch-card grid-margin">
                <div class="card">
                  <div class="card-body">
                    <div class="row">
                      <div class="col-sm-4">
                        <div class="card mb-3 mb-sm-0">
                          <div class="card-body py-3 px-4">
                            <p class="m-0 survey-head">Today Sales</p>
                            <div class="d-flex justify-content-between align-items-end flot-bar-wrapper">
                              <div>
                                <h3 class="m-0 survey-value">5,300</h3>
                              </div>
                              <div id="earningChart" class="flot-chart" style="padding: 0px;"><canvas class="flot-base" width="80" height="63" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 64px; height: 51px;"></canvas><canvas class="flot-overlay" width="80" height="63" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 64px; height: 51px;"></canvas></div>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class="col-sm-4">
                        <div class="card mb-3 mb-sm-0">
                          <div class="card-body py-3 px-4">
                            <p class="m-0 survey-head">Today Purchase</p>
                            <div class="d-flex justify-content-between align-items-end flot-bar-wrapper">
                              <div>
                                <h3 class="m-0 survey-value">9,100</h3>
                              </div>
                              <div id="productChart" class="flot-chart" style="padding: 0px;"><canvas class="flot-base" width="80" height="63" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 64px; height: 51px;"></canvas><canvas class="flot-overlay" width="80" height="63" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 64px; height: 51px;"></canvas></div>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class="col-sm-4">
                        <div class="card">
                          <div class="card-body py-3 px-4">
                            <p class="m-0 survey-head">Today Expenes</p>
                            <div class="d-flex justify-content-between align-items-end flot-bar-wrapper">
                              <div>
                                <h3 class="m-0 survey-value">4,354</h3>
                              </div>
                              <div id="orderChart" class="flot-chart" style="padding: 0px;"><canvas class="flot-base" width="80" height="63" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 64px; height: 51px;"></canvas><canvas class="flot-overlay" width="80" height="63" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 64px; height: 51px;"></canvas></div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="row my-3">
                      <div class="col-sm-12">
                        <div class="flot-chart-wrapper">
                          <div id="flotChart" class="flot-chart" style="padding: 0px;">
                            <canvas class="flot-base" width="1095" height="345" style="width: 876.5px; height: 276px;"></canvas>
                          <canvas class="flot-overlay" width="1095" height="345" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 876.5px; height: 276px;"></canvas><div class="flot-svg" style="position: absolute; top: 0px; left: 0px; height: 100%; width: 100%; pointer-events: none;"><svg style="width: 100%; height: 100%;"><g class="flot-x-axis flot-x1-axis xAxis x1Axis" style="position: absolute; inset: 0px;"><text x="0.46249961853027344" y="271.19999980926514" class="flot-tick-label tickLabel" style="position: absolute; text-align: center;">2000</text><text x="741.6650312641" y="271.19999980926514" class="flot-tick-label tickLabel" style="position: absolute; text-align: center;">5500</text><text x="106.34857556789737" y="271.19999980926514" class="flot-tick-label tickLabel" style="position: absolute; text-align: center;">2500</text><text x="212.23465151726447" y="271.19999980926514" class="flot-tick-label tickLabel" style="position: absolute; text-align: center;">3000</text><text x="318.12072746663154" y="271.19999980926514" class="flot-tick-label tickLabel" style="position: absolute; text-align: center;">3500</text><text x="424.00680341599866" y="271.19999980926514" class="flot-tick-label tickLabel" style="position: absolute; text-align: center;">4000</text><text x="529.8928793653657" y="271.19999980926514" class="flot-tick-label tickLabel" style="position: absolute; text-align: center;">4500</text><text x="635.7789553147328" y="271.19999980926514" class="flot-tick-label tickLabel" style="position: absolute; text-align: center;">5000</text></g></svg></div></div>
                            <div class="custom-legend" style="margin-top:8px;">
                                <span style="background:#bcc1f3; width:12px; height:12px; display:inline-block; margin-right:5px;"></span>
                                <span style="color:#bcc1f3; font-size:12px;">Purchase</span>
                                <span style="background:#3f50f6; width:12px; height:12px; display:inline-block; margin-left:15px; margin-right:5px;"></span>
                                <span style="color:#3f50f6; font-size:12px;">Expenses</span>
                                <span style="background:#ffab2d; width:12px; height:12px; display:inline-block; margin-left:15px; margin-right:5px;"></span>
                                <span style="color:#ffab2d; font-size:12px;">Sales</span>
                            </div>
                        </div>
                      </div>
                    </div>                    
                  </div>
                </div>
              </div>
            </div>
        </div>
        <footer class="footer">
            <div class="d-sm-flex justify-content-center justify-content-sm-between">
                <span class="text-muted d-block text-center text-sm-left d-sm-inline-block">Copyright © AzkoIT 2025</span>
            </div>
        </footer>
    </div>
</asp:Content>

